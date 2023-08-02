namespace GroceryStoreCore.DTOs
{
    public class OrderProductDto
    {
        public Guid OrderId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int Quantity { get; set; }
        public decimal BuyingPrice { get; set; }
        public string ImagePath { get; set; }
        public DateTime OrderDateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
