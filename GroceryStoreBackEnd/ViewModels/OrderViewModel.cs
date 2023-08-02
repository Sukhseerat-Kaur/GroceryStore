namespace GroceryStoreBackEnd.ViewModels
{
    public class OrderViewModel
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal BuyingPrice { get; set; }
    }
}