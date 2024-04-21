namespace Sales_Inventory.Models.DTO
{
    public class DashboardDTO
    {
        public List<RevenueDetailsDTO> daily_revenue { get; set; }
        public List<RevenueDetailsDTO> monthly_revenue { get; set; }
        public List<TopSalesDTO> top_sales { get; set; }
        public List<TopDebtorDTO> top_debtor { get; set; }
    }

    public class RevenueDetailsDTO
    {
        public string month { get; set; } = "";
        public decimal sales { get; set; }
        public decimal expenses { get; set; }
        public decimal credit { get; set; }
        public decimal revenue => (sales + credit) - expenses;
    }

    public class TopSalesDTO
    {
        public string product_name { get; set; }
        public decimal total_sold { get; set; }
    }
    public class TopDebtorDTO
    {
        public string customer_name { get; set; }
        public decimal total_credit { get; set; }
    }
}
