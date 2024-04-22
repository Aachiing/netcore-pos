using System;
using System.Collections.Generic;

namespace Sales_Inventory.Models
{
    public partial class TblOrderHeader
    {
        public int Id { get; set; }
        public string OrderNo { get; set; } = null!;
        public string? CustomerName { get; set; }
        public string? CustomerTin { get; set; }
        public string? CustomerAddress { get; set; }
        public string? Cashier { get; set; }
        public int CashierId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int? PosId { get; set; }
        public decimal? AmountPaid { get; set; }
        public decimal? Discount { get; set; }
        public string? DiscountRemarks { get; set; }
        public decimal Gross { get; set; }
        public decimal? Net { get; set; }
        public decimal? Vat { get; set; }
        public int TotalItems { get; set; }
        public string PaymentType { get; set; } = null!;
        public string? CardNo { get; set; }
        public string? ReferenceNo { get; set; }
        public string? CheckNo { get; set; }
        public decimal? CheckAmount { get; set; }
        public DateTime? CheckDate { get; set; }
    }
}
