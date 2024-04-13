using System;
using System.Collections.Generic;

namespace Sales_Inventory.Models
{
    public partial class DboTblCustomer
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string ContactNo { get; set; } = null!;
        public decimal TotalCredit { get; set; }
    }
}
