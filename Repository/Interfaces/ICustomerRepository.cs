using Sales_Inventory.Models.DTO;

namespace Sales_Inventory.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        Task Create(CustomerDTO dto);
        Task Update(CustomerDTO dto);
        Task<PaginationDTO<CustomerDTO>> List(int page = 0, int page_size = 10, string keyword = "");
        Task<CustomerDTO> GetById(int id);
        Task Delete(int id);
        Task<bool> ExistingCustomer(CustomerDTO dto);
        Task<List<CustomerDTO>> CustomerDDL();
    }
}
