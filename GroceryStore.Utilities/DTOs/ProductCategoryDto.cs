namespace GroceryStoreCore.DTOs
{
    public class ProductCategoryDto
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        public ProductCategoryDto(int productId, int categoryId)
        {
            ProductId = productId;
            CategoryId = categoryId;
        }
    }
}
