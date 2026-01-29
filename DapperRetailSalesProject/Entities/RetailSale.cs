namespace DapperRetailSalesProject.Entities
{
    public class RetailSale
    {
        public int Id { get; set; }
        public string StoreFormat { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }

        public string CustomerType { get; set; }
        public string PaymentMethod { get; set; }
        public string City { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; } // SQL'deki computed column
        public DateTime SaleDate { get; set; }
    }
}
