namespace Sales_Inventory.Models.DTO
{
    public class ReportDTO
    {
    }
    public class DailySalesReportDTO
    {
        public string order_no { get; set; }
        public string customer_name { get; set; }
        public int item_count { get; set; }
        public decimal net { get; set; }
        public decimal amount { get; set; }
        public decimal discount { get; set; }
        public string payment_type { get; set; }
        public string remarks { get; set; }
        public DateTime payment_date { get; set; }
    }
    public class DailyExpensesReportDTO
    {
        public string type { get; set; }
        public string receiver { get; set; }
        public decimal amount { get; set; }
        public string user { get; set; }
        public DateTime expense_date { get; set; }
    }
}
