using System;
using System.Collections.Generic;

namespace Sales_Inventory.Models
{
    public partial class TblReceipt
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public DateTime? DateAdded { get; set; }
        public int? AddedBy { get; set; }

        public virtual TblUser? AddedByNavigation { get; set; }
    }
}
