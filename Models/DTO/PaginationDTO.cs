namespace Sales_Inventory.Models.DTO
{
    public class PaginationDTO<T>
    {
        public int page_count { get; set; }
        public List<T> item_list { get; set; }
    }
}
