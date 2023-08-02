namespace GroceryStoreCore.DTOs
{
    public class ReviewUserDto
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public int ProductId { get; set; }

        public DateTime Time { get; set; }

        public string ReviewString { get; set; }
    }
}