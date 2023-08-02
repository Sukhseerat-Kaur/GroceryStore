using System.ComponentModel.DataAnnotations;

namespace GroceryStore.DataLayer.Entities
{
    public class ProductCategoryEntity
    {
        [Required]
        public int ProductId {get; set;}

        [Required]
        public int CategoryId { get; set;}
    }
}
