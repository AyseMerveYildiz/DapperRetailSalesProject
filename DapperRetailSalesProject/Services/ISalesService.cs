using DapperRetailSalesProject.Dtos;
using DapperRetailSalesProject.Entities;

namespace DapperRetailSalesProject.Services
{
    public interface ISalesService
    {
        // Dashboard verisi getiren metod (Burası aynı kaldı)
        Task<DashboardDto> GetDashboardDataAsync(string storeFormat = null, string category = null, DateTime? startDate = null, DateTime? endDate = null);

        // --- GÜNCELLENEN METOD ---
        // Yeni filtre parametrelerini (ID, Ürün Adı, Fiyat, Tarih) buraya ekledik.
        Task<List<RetailSale>> GetPagedSalesAsync(
            int page,
            int pageSize,
            int? searchId = null,
            string storeFormat = null,
            string category = null,
            string productName = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            DateTime? startDate = null,
            DateTime? endDate = null
        );
    }
}