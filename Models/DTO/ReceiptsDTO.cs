namespace Sales_Inventory.Models.DTO
{
    public class ReceiptsDTO
    {
        public int receipt_id { get; set; }
        public string type { get; set; }
        public string or_from { get; set; }
        public string or_to { get; set; }
        public DateTime date_added { get; set; }
        public int added_by { get; set; }
        public string added_by_name { get; set; } = "";
    }
}
