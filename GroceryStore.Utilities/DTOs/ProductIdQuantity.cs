namespace GroceryStoreCore.DTOs
{
    public class ProductIdQuantity
    {
        public ProductIdQuantity(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}