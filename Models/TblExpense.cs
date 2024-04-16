using System;
using System.Collections.Generic;

namespace Sales_Inventory.Models
{
    public partial class TblExpense
    {
        public int Id { get; set; }
        public string ExpenseType { get; set; } = null!;
        public string Receiver { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public int UserId { get; set; }

        public virtual TblUser User { get; set; } = null!;
    }
}
