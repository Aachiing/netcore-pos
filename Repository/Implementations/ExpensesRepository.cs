using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sales_Inventory.Models;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Interfaces;
using System.Globalization;

namespace Sales_Inventory.Repository.Implementations
{
    public class ExpensesRepository : IExpensesRepository
    {
        private readonly salesinventory_dbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExpensesRepository(salesinventory_dbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Create(ExpensesDTO dto)
        {
            Session session = JsonConvert.DeserializeObject<Session>(_httpContextAccessor.HttpContext!.Session.GetString("UserSession")!)!;

            var obj = new TblExpense();
            obj.ExpenseType = dto.expense_type;
            obj.Receiver = dto.receiver;
            obj.Amount = dto.amount;
            obj.ExpenseDate = dto.expense_date;
            obj.UserId = session.user_id;

            await _context.TblExpenses.AddAsync(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginationDTO<ExpensesDTO>> List(int page = 0, int page_size = 10, string keyword = "")
        {
            PaginationDTO<ExpensesDTO> dto = new PaginationDTO<ExpensesDTO>();

            decimal page_count = _context.TblExpenses.Count();
            page = page - 1;

            dto.page_count = (int)Math.Ceiling(page_count / page_size);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                dto.item_list = await _context.TblExpenses.Where(w => w.ExpenseType.Contains(keyword)).Select(s => new ExpensesDTO
                {
                    id = s.Id,
                    expense_type = s.ExpenseType,
                    receiver = s.Receiver,
                    amount = s.Amount,
                    expense_date = s.ExpenseDate,
                    user = s.User.Fullname!
                }).Skip(page * page_size).Take(page_size).ToListAsync();
            }
            else
            {
                dto.item_list = await _context.TblExpenses.Select(s => new ExpensesDTO
                {
                    id = s.Id,
                    expense_type = s.ExpenseType,
                    receiver = s.Receiver,
                    amount = s.Amount,
                    expense_date = s.ExpenseDate,
                    user = s.User.Fullname!
                }).Skip(page * page_size).Take(page_size).ToListAsync();
            }

            return dto;
        }
    }
}
