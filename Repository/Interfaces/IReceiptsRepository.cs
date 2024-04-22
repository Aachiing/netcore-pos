using Sales_Inventory.Models.DTO;

namespace Sales_Inventory.Repository.Interfaces
{
    public interface IReceiptsRepository
    {
        Task Create(ReceiptsDTO dto);
        Task Update(ReceiptsDTO dto);
        Task<ReceiptsDTO> GetById(int id);
        Task Delete(int id);
        Task<PaginationDTO<ReceiptsDTO>> List(int page = 0, int page_size = 10, string keyword = "");
    }
}
