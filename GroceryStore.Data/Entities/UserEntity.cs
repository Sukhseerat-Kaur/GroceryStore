using System.ComponentModel.DataAnnotations;

namespace GroceryStore.DataLayer.Entities
{
    public class UserEntity
    {
        [Key] 
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please Enter User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        public string UserEmail { get; set; }

        [Required, MinLength(10), MaxLength(10)]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string UserRole { get; set; }

    }
}
