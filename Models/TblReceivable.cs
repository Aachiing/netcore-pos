using System;
using System.Collections.Generic;

namespace Sales_Inventory.Models
{
    public partial class TblReceivable
    {
        public int Id { get; set; }
        public int CreditHeaderId { get; set; }
        public string OrderNo { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public decimal AmountPaid { get; set; }
        public string PaymentType { get; set; } = null!;
        public string? CardNo { get; set; }
        public string? ReferenceNo { get; set; }
        public string? CheckNo { get; set; }
        public DateTime? CheckDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public int CashierId { get; set; }
        public string? Cashier { get; set; }
    }
}
