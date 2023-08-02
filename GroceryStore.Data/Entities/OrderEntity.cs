using System.ComponentModel.DataAnnotations;

namespace GroceryStore.DataLayer.Entities
{
    public class OrderEntity
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal BuyingPrice { get; set; }

        [Required]
        public DateTime OrderDateTime { get; set; }
    }
}

//productid, name, image, price, 