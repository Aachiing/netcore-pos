using Microsoft.EntityFrameworkCore;
using Sales_Inventory.Models;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Interfaces;
using System.Globalization;

namespace Sales_Inventory.Repository.Implementations
{
    public class ExpensesRepository : IExpensesRepository
    {
        private readonly salesinventory_dbContext _context;

        public ExpensesRepository(salesinventory_dbContext context) { _context = context; }

        public async Task Create(ExpensesDTO dto)
        {
            var obj = new TblExpense();
            obj.ExpenseType = dto.expense_type;
            obj.Receiver = dto.receiver;
            obj.Amount = dto.amount;
            obj.ExpenseDate = dto.expense_date;
            obj.UserId = dto.user_id;

            await _context.TblExpenses.AddAsync(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginationDTO<ExpensesDTO>> List(int page = 0, int page_size = 10, string keyword = "")
        {
            PaginationDTO<ExpensesDTO> dto = new PaginationDTO<ExpensesDTO>();

            decimal page_count = _context.TblCustomers.Count();
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
                    user_id = s.UserId
                }).Skip(page * page_size).Take(page_size).ToListAsync();

                return dto;
            }

            dto.item_list = await _context.TblExpenses.Select(s => new ExpensesDTO
            {
                id = s.Id,
                expense_type = s.ExpenseType,
                receiver = s.Receiver,
                amount = s.Amount,
                expense_date = s.ExpenseDate,
                user_id = s.UserId
            }).Skip(page * page_size).Take(page_size).ToListAsync();

            return dto;
        }
    }
}
