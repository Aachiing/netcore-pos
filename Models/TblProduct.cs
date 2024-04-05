using System;
using System.Collections.Generic;

namespace Sales_Inventory.Models
{
    public partial class TblProduct
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Barcode { get; set; }
        public string? Brand { get; set; }
        public int? Quantity { get; set; }
        public string? Unit { get; set; }
        public decimal? Price { get; set; }
        public bool? IsDiscounted { get; set; }
        public decimal? DiscountRate { get; set; }
    }
}
