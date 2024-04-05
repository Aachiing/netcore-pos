using Sales_Inventory.Models.DTO;

namespace Sales_Inventory.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO> Login(UserDTO dto);
    }
}
