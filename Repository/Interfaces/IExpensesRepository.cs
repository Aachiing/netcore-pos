using Sales_Inventory.Models.DTO;

namespace Sales_Inventory.Repository.Interfaces
{
    public interface IExpensesRepository
    {
        Task Create(ExpensesDTO dto);
        Task<PaginationDTO<ExpensesDTO>> List(int page = 0, int page_size = 10, string keyword = "");
    }
}
