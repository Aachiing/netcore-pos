using Microsoft.EntityFrameworkCore;
using Sales_Inventory.Models;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Interfaces;

namespace Sales_Inventory.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly salesinventory_dbContext _context;

        public UserRepository(salesinventory_dbContext context) { _context = context; }
        public async Task<UserDTO> Login(UserDTO dto)
        {
#pragma warning disable CS8603 // Possible null reference return.

            return await _context.TblUsers.Where(w => w.Username == dto.user_name && w.Password == dto.password)
                             .Select(s => new UserDTO
                             {
                                 id = s.Id,
                                 full_name = s.Fullname!,
                                 user_name = s.Username!,
                                 user_type = s.Username!
                             }).SingleOrDefaultAsync();

#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
