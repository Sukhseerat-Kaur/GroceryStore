namespace GroceryStoreBackEnd.ViewModels
{
    public class ReviewViewModel
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public DateTime Time { get; set; }

        public string ReviewString { get; set; }
    }
}