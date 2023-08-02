using System.ComponentModel.DataAnnotations;

namespace GroceryStore.DataLayer.Entities
{
    public class ProductEntity
    {
        [Key] 
        public int ProductId { get; set; }

        [Required, MaxLength(100)]
        public string ProductName { get; set; }

        [Required, MaxLength(255)]
        public string ProductDescription { get; set; }

        //[Required, MaxLength(100)]
        //public string Categories { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        [Required]
        public int ProductQuantity { get; set; }

        public decimal ProductDiscount { get; set; } = 0;

        [Required]
        [MaxLength(255)]
        public string ImagePath { get; set; }

        [Required]
        public bool IsDeleted { get; set; }=false;
    }
}
