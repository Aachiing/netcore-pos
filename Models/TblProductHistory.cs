using System;
using System.Collections.Generic;

namespace Sales_Inventory.Models
{
    public partial class TblProductHistory
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Barcode { get; set; }
        public string? Brand { get; set; }
        public int? OldQuantity { get; set; }
        public int? AddedQuantity { get; set; }
        public int? NewQuantity { get; set; }
        public string? Unit { get; set; }
        public decimal? Price { get; set; }
        public bool? IsDiscounted { get; set; }
        public decimal? DiscountRate { get; set; }
        public DateTime? DateAdded { get; set; }
        public int? AddedBy { get; set; }

        public virtual TblUser? AddedByNavigation { get; set; }
    }
}
