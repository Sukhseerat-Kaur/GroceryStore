namespace GroceryStoreCore.DTOs
{
    public class UserWithoutPasswordDto
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public string PhoneNumber { get; set; }

        public string UserRole { get; set; }
    }
}
