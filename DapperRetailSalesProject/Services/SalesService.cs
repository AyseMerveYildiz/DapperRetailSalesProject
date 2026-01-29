using Dapper;
using DapperRetailSalesProject.Context;
using DapperRetailSalesProject.Dtos;
using DapperRetailSalesProject.Entities;
using System.Data;

namespace DapperRetailSalesProject.Services
{
    public class SalesService : ISalesService
    {
        private readonly DapperContext _context;
        public SalesService(DapperContext context) { _context = context; }

        // Dashboard Metodu (Mevcut kodun aynısı - dokunmadık)
        public async Task<DashboardDto> GetDashboardDataAsync(string storeFormat = null, string category = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var conditions = new List<string>();
            if (!string.IsNullOrEmpty(storeFormat)) conditions.Add("StoreFormat = @Format");
            if (!string.IsNullOrEmpty(category)) conditions.Add("CategoryName = @Cat");
            if (startDate.HasValue) conditions.Add("SaleDate >= @Start");
            if (endDate.HasValue) conditions.Add("SaleDate <= @End");

            string whereClause = conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : "";

            var query = $@"
                SELECT COUNT(*) FROM RetailSales {whereClause}; 
                SELECT ISNULL(SUM(TotalPrice), 0) FROM RetailSales {whereClause}; 
                SELECT StoreFormat as Label, SUM(TotalPrice) as Value FROM RetailSales {whereClause} GROUP BY StoreFormat; 
                SELECT TOP 5 CategoryName as Label, SUM(TotalPrice) as Value FROM RetailSales {whereClause} GROUP BY CategoryName ORDER BY Value DESC;
                SELECT TOP 1 ProductName FROM RetailSales {whereClause} GROUP BY ProductName ORDER BY COUNT(*) DESC;
                SELECT COUNT(DISTINCT StoreFormat) * 125 FROM RetailSales {whereClause};
                SELECT ISNULL(SUM(TotalPrice), 0) FROM RetailSales WHERE StoreFormat = 'S-Online';
                SELECT ISNULL(AVG(TotalPrice), 0) FROM RetailSales {whereClause};
                SELECT TOP 1 StoreFormat FROM RetailSales {whereClause} GROUP BY StoreFormat ORDER BY SUM(TotalPrice) DESC;
                SELECT City as Label, SUM(TotalPrice) as Value FROM RetailSales {whereClause} GROUP BY City ORDER BY Value DESC;
                SELECT TOP 30 FORMAT(SaleDate, 'dd.MM.yyyy') as Label, SUM(TotalPrice) as Value FROM RetailSales {whereClause} GROUP BY FORMAT(SaleDate, 'dd.MM.yyyy'), CAST(SaleDate AS DATE) ORDER BY CAST(SaleDate AS DATE) DESC;";

            using var connection = _context.CreateConnection();
            using var multi = await connection.QueryMultipleAsync(new CommandDefinition(query, new { Format = storeFormat, Cat = category, Start = startDate, End = endDate }, commandTimeout: 300));

            var dto = new DashboardDto();
            dto.TotalOrderCount = await multi.ReadFirstAsync<int>();
            dto.TotalRevenue = await multi.ReadFirstAsync<decimal>();
            dto.FormatDistribution = (await multi.ReadAsync<ChartDataDto>()).ToList();
            dto.TopCategories = (await multi.ReadAsync<ChartDataDto>()).ToList();
            dto.MostSoldProduct = (await multi.ReadFirstOrDefaultAsync<string>()) ?? "Veri Yok";
            dto.ActiveStoreCount = await multi.ReadFirstAsync<int>();
            dto.SanalMarketRevenue = await multi.ReadFirstOrDefaultAsync<decimal>();
            dto.AverageOrderValue = await multi.ReadFirstAsync<decimal>();
            dto.TopStoreName = (await multi.ReadFirstOrDefaultAsync<string>()) ?? "Veri Yok";
            dto.CityDistribution = (await multi.ReadAsync<ChartDataDto>()).ToList();
            dto.SalesTrend = (await multi.ReadAsync<ChartDataDto>()).ToList();
            dto.SalesTrend.Reverse();

            return dto;
        }

        // --- GÜNCELLENEN LİSTELEME METODU ---
        public async Task<List<RetailSale>> GetPagedSalesAsync(
            int page,
            int pageSize,
            int? searchId = null,
            string storeFormat = null,
            string category = null,
            string productName = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var conditions = new List<string>();
            var parameters = new DynamicParameters();

            // Sayfalama Parametreleri
            parameters.Add("Skip", (page - 1) * pageSize);
            parameters.Add("Take", pageSize);

            // 1. ID Filtresi
            if (searchId.HasValue)
            {
                conditions.Add("Id = @SearchId");
                parameters.Add("SearchId", searchId.Value);
            }

            // 2. Format Filtresi
            if (!string.IsNullOrEmpty(storeFormat))
            {
                conditions.Add("StoreFormat = @Format");
                parameters.Add("Format", storeFormat);
            }

            // 3. Kategori Filtresi
            if (!string.IsNullOrEmpty(category))
            {
                conditions.Add("CategoryName = @Category");
                parameters.Add("Category", category);
            }

            // 4. Ürün Adı (İçerir - LIKE)
            if (!string.IsNullOrEmpty(productName))
            {
                conditions.Add("ProductName LIKE @ProductName");
                parameters.Add("ProductName", $"%{productName}%");
            }

            // 5. Fiyat Aralığı
            if (minPrice.HasValue)
            {
                conditions.Add("TotalPrice >= @MinPrice");
                parameters.Add("MinPrice", minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                conditions.Add("TotalPrice <= @MaxPrice");
                parameters.Add("MaxPrice", maxPrice.Value);
            }

            // 6. Tarih Aralığı
            if (startDate.HasValue)
            {
                conditions.Add("SaleDate >= @StartDate");
                parameters.Add("StartDate", startDate.Value);
            }
            if (endDate.HasValue)
            {
                conditions.Add("SaleDate <= @EndDate");
                parameters.Add("EndDate", endDate.Value.Date.AddDays(1).AddTicks(-1)); // Gün sonuna kadar
            }

            string whereClause = conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : "";

            var query = $@"
                SELECT * FROM RetailSales 
                {whereClause} 
                ORDER BY SaleDate DESC 
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

            using var connection = _context.CreateConnection();

            return (await connection.QueryAsync<RetailSale>(query, parameters, commandTimeout: 120)).ToList();
        }
    }
}