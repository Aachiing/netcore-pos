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
    public class InventoryHistoryReportDTO
    {
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string barcode { get; set; }
        public int old_qty { get; set; }
        public int added_qty { get; set; }
        public int new_qty { get; set; }
        public DateTime date_added { get; set; }
        public string added_by { get; set; }
    }
}
