namespace Sales_Inventory.Models.DTO
{
    public class CustomerDTO
    {
        public int id { get; set; }
        public string customer_name { get; set; }
        public string address { get; set; }
        public string contact_no { get; set; }
        public decimal total_credit { get; set; }
    }
}
