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
        public decimal amount { get; set; }
        public string payment_type { get; set; }
        public DateTime payment_date { get; set; }
    }
}
