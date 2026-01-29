using Dapper;
using DapperRetailSalesProject.Entities; // RetailSale sınıfının olduğu yer
using DapperRetailSalesProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace DapperRetailSalesProject.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ISalesService _salesService;
        private readonly string _connectionString; // Veritabanı bağlantısı için

        // Constructor'ı güncelledik: IConfiguration ekledik
        public DashboardController(ISalesService salesService, IConfiguration configuration)
        {
            _salesService = salesService;
            // appsettings.json dosyasındaki ismin "DefaultConnection" olduğunu varsayıyoruz
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IActionResult> Index(string storeFormat, string category, DateTime? startDate, DateTime? endDate)
        {
            var data = await _salesService.GetDashboardDataAsync(storeFormat, category, startDate, endDate);
            ViewBag.SelectedFormat = storeFormat;
            ViewBag.SelectedCategory = category;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            return View(data);
        }

        // --- TAMİR EDİLEN VE ÇALIŞAN EXCEL METODU ---
        public async Task<IActionResult> ExportToExcel(string storeFormat, string category, DateTime? startDate, DateTime? endDate)
        {
            // 1. ADIM: Verileri Filtreli Olarak Çek (Dapper)
            // Servis sadece özet getirdiği için, burada listeyi almak zorundayız.
            var sqlBuilder = new System.Text.StringBuilder("SELECT * FROM RetailSales WHERE 1=1");
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(storeFormat))
            {
                sqlBuilder.Append(" AND StoreFormat = @format");
                parameters.Add("format", storeFormat);
            }

            if (!string.IsNullOrEmpty(category))
            {
                sqlBuilder.Append(" AND CategoryName = @category");
                parameters.Add("category", category);
            }

            if (startDate.HasValue)
            {
                sqlBuilder.Append(" AND SaleDate >= @start");
                parameters.Add("start", startDate.Value);
            }

            if (endDate.HasValue)
            {
                sqlBuilder.Append(" AND SaleDate <= @end");
                // Bitiş tarihini gün sonuna çekiyoruz (23:59:59)
                parameters.Add("end", endDate.Value.AddDays(1).AddSeconds(-1));
            }

            // En son satışlar en üstte olsun
            sqlBuilder.Append(" ORDER BY SaleDate DESC");

            using (var connection = new SqlConnection(_connectionString))
            {
                var sales = await connection.QueryAsync<RetailSale>(sqlBuilder.ToString(), parameters);

                // 2. ADIM: Excel Paketini Oluştur
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage())
                {
                    var ws = package.Workbook.Worksheets.Add("Satış Raporu");

                    // Başlık
                    ws.Cells["A1"].Value = "S-RETAIL DETAYLI SATIŞ RAPORU";
                    ws.Cells["A1:F1"].Merge = true;
                    ws.Cells["A1"].Style.Font.Bold = true;
                    ws.Cells["A1"].Style.Font.Size = 14;
                    ws.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ws.Cells["A2"].Value = $"Rapor Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}";
                    ws.Cells["A2:F2"].Merge = true;
                    ws.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    // Sütun Başlıkları (Satır 4)
                    ws.Cells["A4"].Value = "Fiş ID";
                    ws.Cells["B4"].Value = "Tarih";
                    ws.Cells["C4"].Value = "Format";
                    ws.Cells["D4"].Value = "Kategori";
                    ws.Cells["E4"].Value = "Ürün Adı";
                    ws.Cells["F4"].Value = "Tutar";

                    // Başlıkları Boya (Turuncu)
                    using (var range = ws.Cells["A4:F4"])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 133, 0));
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    // Verileri Bas (Satır 5'ten başla)
                    int row = 5;
                    if (sales != null && sales.Any())
                    {
                        foreach (var item in sales)
                        {
                            ws.Cells[row, 1].Value = item.Id;
                            ws.Cells[row, 2].Value = item.SaleDate.ToString("dd.MM.yyyy HH:mm");
                            ws.Cells[row, 3].Value = item.StoreFormat;
                            ws.Cells[row, 4].Value = item.CategoryName;
                            ws.Cells[row, 5].Value = item.ProductName;
                            ws.Cells[row, 6].Value = item.TotalPrice;
                            ws.Cells[row, 6].Style.Numberformat.Format = "#,##0.00 ₺"; // Para formatı
                            row++;
                        }
                        // Sütunları genişlet
                        ws.Cells.AutoFitColumns();
                    }
                    else
                    {
                        ws.Cells["A5"].Value = "Kayıt bulunamadı.";
                    }

                    var fileContents = package.GetAsByteArray();
                    var fileName = $"S-Retail_SatisRaporu_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
                    return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        // --- GÜNCELLENEN LIST ACTION (Senin kodun aynen korundu) ---
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

            var sales = await _salesService.GetPagedSalesAsync(
                page, pageSize, searchId, storeFormat, category, productName, minPrice, maxPrice, startDate, endDate);

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