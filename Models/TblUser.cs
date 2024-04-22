using System;
using System.Collections.Generic;

namespace Sales_Inventory.Models
{
    public partial class TblUser
    {
        public TblUser()
        {
            TblExpenses = new HashSet<TblExpense>();
            TblProductHistories = new HashSet<TblProductHistory>();
            TblReceipts = new HashSet<TblReceipt>();
        }

        public int Id { get; set; }
        public string? Fullname { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsActive { get; set; }
        public string? Usertype { get; set; }

        public virtual ICollection<TblExpense> TblExpenses { get; set; }
        public virtual ICollection<TblProductHistory> TblProductHistories { get; set; }
        public virtual ICollection<TblReceipt> TblReceipts { get; set; }
    }
}
