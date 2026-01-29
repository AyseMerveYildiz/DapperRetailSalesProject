namespace DapperRetailSalesProject.Entities
{
    public class Store
    {
        public int Id { get; set; }
        public string StoreName { get; set; } // Örn: Kadıköy Şubesi
        public string StoreFormat { get; set; } // Örn: Jet, M, 5M
        public string City { get; set; }
    }
}
