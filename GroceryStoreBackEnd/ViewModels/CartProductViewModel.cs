namespace GroceryStoreBackEnd.ViewModels
{
    public class CartProductViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal ProductDiscount { get; set; } = 0;

        public int ProductQuantityInCart { get; set; } = 1;

        public bool IsDeleted { get; set; }

    }
}
