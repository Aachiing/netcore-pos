namespace Sales_Inventory.Models.DTO
{
    public class OrderHeaderDTO
    {
        public int order_id { get; set; }
        public string order_no { get; set; }
        public string customer_name { get; set; }
        public string customer_tin { get; set; }
        public string customer_address { get; set; }
        public string cashier { get; set; }
        public int cashier_id { get; set; }
        public DateTime transaction_date { get; set; } = DateTime.Now;
        public string transaction_type { get; set; }
        public int pos_id { get; set; }
        public decimal amount_paid { get; set; }
        public decimal discount { get; set; }
        public string remarks { get; set; }
        public decimal balance { get; set; }
        public decimal gross { get; set; }
        public decimal net { get; set; }
        public decimal vat { get; set; }
        public int total_items { get; set; }
        public string payment_type { get; set; }
        public string card_no { get; set; }
        public string reference_no { get; set; }
        public string check_no { get; set; }
        public decimal check_amount { get; set; }
        public DateTime check_date { get; set; }
        public List<OrderDetailsDTO> order_details { get; set; }
    }

    public class OrderDetailsDTO
    {
        public int order_id { get; set; }
        public int product_id { get; set; }
        public string order_no { get; set; }
        public int order_header_id { get; set; }
        public string item_name { get; set; }
        public string barcode { get; set; }
        public int quantity { get; set; }
        public decimal total_amount { get; set; }
        public string unit { get; set; }
        public decimal unit_price { get; set; }
        public decimal discount { get; set; }
        public decimal discount_rate { get; set; }
    }
}


