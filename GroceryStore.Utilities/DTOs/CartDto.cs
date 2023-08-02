namespace GroceryStoreCore.DTOs
{
    public class CartDto
    {
        public CartDto(int productId, int userId, int quantity)
        {
            ProductId = productId;
            UserId = userId;
            Quantity = quantity;
        }
        public int UserId;

        public int ProductId;

        public int Quantity = 1;
    }
}
