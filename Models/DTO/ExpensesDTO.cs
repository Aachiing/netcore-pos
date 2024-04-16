namespace Sales_Inventory.Models.DTO
{
    public class ExpensesDTO
    {
        public int id { get; set; }
        public string expense_type { get; set; } = null!;
        public string receiver { get; set; } = null!;
        public decimal amount { get; set; }
        public DateTime expense_date { get; set; }
        public int user_id { get; set; }
        public string user { get; set; } = "";
    }
}
