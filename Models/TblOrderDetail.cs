using System;
using System.Collections.Generic;

namespace Sales_Inventory.Models
{
    public partial class TblOrderDetail
    {
        public int Id { get; set; }
        public string OrderNo { get; set; } = null!;
        public int OrderHeaderId { get; set; }
        public string ItemName { get; set; } = null!;
        public string Barcode { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountRate { get; set; }
    }
}
