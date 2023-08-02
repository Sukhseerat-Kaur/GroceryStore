using System.ComponentModel.DataAnnotations;

namespace GroceryStore.DataLayer.Entities
{
    public class ReviewEntity
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int ProductId { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public string ReviewString { get; set; }
    }
}
