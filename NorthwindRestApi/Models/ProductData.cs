namespace NorthwindRestApi.Models
{
    public class ProductData
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = "default";
        public string CategoryName { get; set; } = "default";

        public string SupplierName { get; set; } = "default";
    }
}
