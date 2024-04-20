using Humanizer;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sales_Inventory.Models;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Interfaces;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Sales_Inventory.Repository.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly salesinventory_dbContext _context;

        public CustomerRepository(salesinventory_dbContext context) { _context = context; }

        public async Task Create(CustomerDTO dto)
        {
            var obj = new TblCustomer();
            obj.CustomerName = dto.customer_name;
            obj.Address = dto.address;
            obj.ContactNo = dto.contact_no;
            obj.IsDeleted = false;
            obj.TotalCredit = 0;

            await _context.TblCustomers.AddAsync(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CustomerDTO>> CustomerDDL()
        {
            return await _context.TblCustomers.Select(s => new CustomerDTO
            {
                id = s.Id,
                customer_name = s.CustomerName
            }).ToListAsync()!;
        }

        public async Task Delete(int id)
        {
            var customer = await _context.TblCustomers.Where(w => w.Id == id).FirstOrDefaultAsync();

            if (customer != null)
            {
                customer.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistingCustomer(CustomerDTO dto)
        {
            return await _context.TblCustomers.AnyAsync(a => a.CustomerName == dto.customer_name && a.IsDeleted == false);
        }

        public async Task<CustomerDTO> GetById(int id)
        {
            return await _context.TblCustomers.Where(w => w.Id == id).Select(s => new CustomerDTO
            {
                id = s.Id,
                customer_name = s.CustomerName,
                address = s.Address,
                contact_no = s.ContactNo,
                total_credit = s.TotalCredit,
            }).FirstOrDefaultAsync()!;
        }

        public async Task<PaginationDTO<CustomerDTO>> List(int page = 0, int page_size = 10, string keyword = "")
        {
            PaginationDTO<CustomerDTO> dto = new PaginationDTO<CustomerDTO>();

            decimal page_count = _context.TblCustomers.Count();
            page = page - 1;

            dto.page_count = (int)Math.Ceiling(page_count / page_size);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                dto.item_list = await _context.TblCustomers.Where(w => w.CustomerName.Contains(keyword) && w.IsDeleted == false).Select(s => new CustomerDTO
                {
                    id = s.Id,
                    customer_name = s.CustomerName,
                    address = s.Address,
                    contact_no = s.ContactNo,
                    total_credit = s.TotalCredit,
                }).Skip(page * page_size).Take(page_size).ToListAsync();
            }
            else
            {
                dto.item_list = await _context.TblCustomers.Where(w => w.IsDeleted == false).Select(s => new CustomerDTO
                {
                    id = s.Id,
                    customer_name = s.CustomerName,
                    address = s.Address,
                    contact_no = s.ContactNo,
                    total_credit = s.TotalCredit,
                }).Skip(page * page_size).Take(page_size).ToListAsync();
            }

            return dto;
        }

        public async Task Update(CustomerDTO dto)
        {
            var customer = await _context.TblCustomers.Where(w => w.Id == dto.id).FirstOrDefaultAsync();

            if (customer != null)
            {
                customer.CustomerName = dto.customer_name;
                customer.Address = dto.address;
                customer.ContactNo = dto.contact_no;

                await _context.SaveChangesAsync();
            }
        }
    }
}
