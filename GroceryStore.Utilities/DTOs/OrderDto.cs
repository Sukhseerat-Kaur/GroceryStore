namespace GroceryStoreCore.DTOs
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public decimal BuyingPrice { get; set; }
        public DateTime OrderDateTime { get; set; }

    }
}
