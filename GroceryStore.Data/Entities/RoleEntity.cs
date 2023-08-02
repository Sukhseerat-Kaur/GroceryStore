using System.ComponentModel.DataAnnotations;

namespace GroceryStore.DataLayer.Entities
{
    public class RoleEntity
    {
        [Key]
        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }
}
