using System.ComponentModel.DataAnnotations;

namespace Sales_Inventory.Models.DTO
{
    public class UserDTO
    {
        public int id { get; set; }
        public string? full_name { get; set; }

        [Required]
        public string user_name { get; set; }
        public string? user_type { get; set; }

        [Required]
        public string? password { get; set; }
    }
}
