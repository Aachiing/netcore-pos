using Sales_Inventory.Models.DTO;

namespace Sales_Inventory.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task Create(ProductDTO dto);
        Task Update(ProductDTO dto);
        Task<PaginationDTO<ProductDTO>> List(int page = 0, int page_size = 10, string keyword = "");
        Task<ProductDTO> GetById(int id);
        Task Delete(int id);
        Task<bool> ExistingProduct(ProductDTO dto);
        Task<IEnumerable<ProductDTO>> Search(string keyword);
    }
}
