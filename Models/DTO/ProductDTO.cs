using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Sales_Inventory.Models.DTO
{
    public class ProductDTO
    {
        public int product_id { get; set; }

        [Required(ErrorMessage = "Product name is required!")]
        public string product_name { get; set; }

        [Required(ErrorMessage = "Product code is required!")]
        public string product_code { get; set; }

        [Required(ErrorMessage = "Barcode is required!")]
        public string barcode { get; set; }
        public string brand { get; set; }

        [Range(0, int.MaxValue)]
        public int quantity { get; set; }

        [Range(0, int.MaxValue)]
        public int additional_quantity { get; set; }
        public string unit { get; set; }

        [Range(0, int.MaxValue)]
        public decimal price { get; set; }
        public bool is_discounted { get; set; }

        [Range(0, int.MaxValue)]
        public decimal discount_rate { get; set; }

        public IEnumerable<ProductDTO>? list { get; set; }
    }
}
