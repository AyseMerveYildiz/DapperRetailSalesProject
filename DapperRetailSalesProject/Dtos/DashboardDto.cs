using DapperRetailSalesProject.Entities;

namespace DapperRetailSalesProject.Dtos
{
    public class DashboardDto
    {
        // Üst Widget Metrikleri
        public int TotalOrderCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int MoneyKartSalesCount { get; set; }
        public int ActiveStoreCount { get; set; }
        public int CategoryCount { get; set; }
        public string TopStoreName { get; set; }

        // Kâr Hesabı (Basit matematiksel işlem)
        public decimal TotalProfit => TotalRevenue * 0.22m;

        // BI Metrikleri (Orta Kartlar)
        public string MostSoldProduct { get; set; }
        public decimal SanalMarketRevenue { get; set; }

        // --- GRAFİK LİSTELERİ ---

        // 1. Format Dağılımı (Pasta Grafik)
        public List<ChartDataDto> FormatDistribution { get; set; } = new();

        // 2. Kategori Listesi
        public List<ChartDataDto> TopCategories { get; set; } = new();

        // 3. Son Satışlar Listesi
        public List<RetailSale> RecentSales { get; set; } = new();

        // 4. Harita Verisi (İl Dağılımı)
        public List<ChartDataDto> CityDistribution { get; set; } = new();

        // 5. YENİ EKLENEN: Satış Trendi (Çizgi Grafik)
        // (Hata veren kısım burasıydı, şimdi ekliyoruz)
        public List<ChartDataDto> SalesTrend { get; set; } = new();
    }
}