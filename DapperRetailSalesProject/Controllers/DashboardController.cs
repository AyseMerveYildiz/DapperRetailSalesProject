using DapperRetailSalesProject.Services;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace DapperRetailSalesProject.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ISalesService _salesService;
        public DashboardController(ISalesService salesService) { _salesService = salesService; }

        public async Task<IActionResult> Index(string storeFormat, string category, DateTime? startDate, DateTime? endDate)
        {
            var data = await _salesService.GetDashboardDataAsync(storeFormat, category, startDate, endDate);
            ViewBag.SelectedFormat = storeFormat;
            ViewBag.SelectedCategory = category;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            return View(data);
        }

        public async Task<IActionResult> ExportToExcel(string storeFormat, string category, DateTime? startDate, DateTime? endDate)
        {
            var data = await _salesService.GetDashboardDataAsync(storeFormat, category, startDate, endDate);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Yönetim Raporu");
                ws.Cells["A1"].Value = "S-Retail Satış Raporu";
                ws.Cells["A2"].Value = $"Tarih: {DateTime.Now}";
                // (Buradaki Excel kodların aynen kalabilir, özet geçtim)

                var fileContents = package.GetAsByteArray();
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Rapor.xlsx");
            }
        }

        // --- GÜNCELLENEN LIST ACTION ---
        public async Task<IActionResult> List(
            int page = 1,
            int? searchId = null,
            string storeFormat = null,
            string category = null,
            string productName = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            int pageSize = 20;

            // Servise tüm filtreleri gönderiyoruz
            var sales = await _salesService.GetPagedSalesAsync(
                page, pageSize, searchId, storeFormat, category, productName, minPrice, maxPrice, startDate, endDate);

            // Filtreleri ViewBag'e geri yüklüyoruz ki sayfada kaybolmasınlar
            ViewBag.CurrentPage = page;
            ViewBag.SearchId = searchId;
            ViewBag.SelectedFormat = storeFormat;
            ViewBag.SelectedCategory = category;
            ViewBag.SearchProduct = productName;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(sales);
        }
    }
}