namespace Sales_Inventory.Models.DTO
{
    public class ReceivableDTO
    {
        public int id { get; set; }
        public int credit_header_id { get; set; }
        public string order_no { get; set; }
        public string customer_name { get; set; }
        public decimal amount_paid { get; set; }
        public string payment_type { get; set; }
        public DateTime payment_date { get; set; }
        public string cashier { get; set; }
        public int cashier_id { get; set; }
        public string card_no { get; set; }
        public string reference_no { get; set; }
        public string check_no { get; set; }
        public DateTime check_date { get; set; }
    }
}
