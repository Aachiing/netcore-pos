using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sales_Inventory.Models;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Interfaces;

namespace Sales_Inventory.Repository.Implementations
{
    public class ReceiptsRepository : IReceiptsRepository
    {
        private readonly salesinventory_dbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReceiptsRepository(salesinventory_dbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Create(ReceiptsDTO dto)
        {
            Session session = JsonConvert.DeserializeObject<Session>(_httpContextAccessor.HttpContext!.Session.GetString("UserSession")!)!;

            var obj = new TblReceipt();
            obj.Type = dto.type;
            obj.From = dto.or_from.PadLeft(8, '0');
            obj.To = dto.or_to.PadLeft(8, '0');
            obj.DateAdded = DateTime.Now;
            obj.AddedBy = session.user_id;

            await _context.TblReceipts.AddAsync(obj);
            await _context.SaveChangesAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ReceiptsDTO> GetById(int id)
        {
            return await _context.TblReceipts.Where(w => w.Id == id).Select(s => new ReceiptsDTO
            {
                receipt_id = s.Id,
                type = s.Type,
                or_from = s.From,
                or_to = s.To,
                date_added = Convert.ToDateTime(s.DateAdded)
            }).FirstOrDefaultAsync()!;
        }

        public async Task<PaginationDTO<ReceiptsDTO>> List(int page = 0, int page_size = 10, string keyword = "")
        {
            PaginationDTO<ReceiptsDTO> dto = new PaginationDTO<ReceiptsDTO>();

            decimal page_count = _context.TblExpenses.Count();
            page = page - 1;

            dto.page_count = (int)Math.Ceiling(page_count / page_size);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                dto.item_list = await _context.TblReceipts.Where(w => w.From.Contains(keyword) || w.To.Contains(keyword)).Select(s => new ReceiptsDTO
                {
                    receipt_id = s.Id,
                    type = s.Type,
                    or_from = s.From,
                    or_to = s.To,
                    date_added = Convert.ToDateTime(s.DateAdded)
                }).Skip(page * page_size).Take(page_size).ToListAsync();
            }
            else
            {
                dto.item_list = await _context.TblReceipts.Select(s => new ReceiptsDTO
                {
                    receipt_id = s.Id,
                    type = s.Type,
                    or_from = s.From,
                    or_to = s.To,
                    date_added = Convert.ToDateTime(s.DateAdded)
                }).Skip(page * page_size).Take(page_size).ToListAsync();
            }

            return dto;
        }

        public async Task Update(ReceiptsDTO dto)
        {
            Session session = JsonConvert.DeserializeObject<Session>(_httpContextAccessor.HttpContext!.Session.GetString("UserSession")!)!;

            var receipt = await _context.TblReceipts.Where(w => w.Id == dto.receipt_id).FirstOrDefaultAsync();

            if (receipt != null)
            {
                receipt.Type = dto.type;
                receipt.From = dto.or_from.PadLeft(8, '0');
                receipt.To = dto.or_to.PadLeft(8, '0');
                receipt.DateAdded = DateTime.Now;
                receipt.AddedBy = session.user_id;

                await _context.SaveChangesAsync();
            }
        }
    }
}
