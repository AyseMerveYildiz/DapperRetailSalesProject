using DapperRetailSalesProject.Context;
using DapperRetailSalesProject.Services;
// using OfficeOpenXml;  <-- BUNU SİL (Artık gerek yok)

var builder = WebApplication.CreateBuilder(args);

// --- PARA BİRİMİ AYARI (TL) ---
var cultureInfo = new System.Globalization.CultureInfo("tr-TR");
cultureInfo.NumberFormat.CurrencySymbol = "₺";
cultureInfo.NumberFormat.CurrencyGroupSeparator = ".";
cultureInfo.NumberFormat.CurrencyDecimalSeparator = ",";
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
// -----------------------------

// Add services...

// Add services to the container.
builder.Services.AddControllersWithViews();
// ... (kodların devamı)


// Servisler
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<ISalesService, SalesService>();

var app = builder.Build();

// Hata yönetimi ve HTTPS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// --- LİSANS KODUNU BURADAN SİLDİK ---
// Artık appsettings.json dosyasından otomatik okuyacak.

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();