using Sales_Inventory.Models.DTO;

namespace Sales_Inventory.Repository.Interfaces
{
    public interface IReturnRepository
    {
        Task<List<OrderDetailsDTO>> LoadOrderDetails(string keyword = "");
    }
}
