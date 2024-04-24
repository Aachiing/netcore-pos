using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using Newtonsoft.Json;
using Sales_Inventory.Models;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Interfaces;
using System.Diagnostics;

namespace Sales_Inventory.Repository.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly salesinventory_dbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductRepository(salesinventory_dbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task Create(ProductDTO dto)
        {
            var obj = new TblProduct();
            obj.Name = dto.product_name;
            obj.Code = dto.product_code;
            obj.Barcode = dto.barcode;
            obj.Brand = dto.brand;
            obj.Unit = dto.unit;
            obj.Price = dto.price;
            obj.Quantity = dto.quantity;
            obj.IsDiscounted = dto.is_discounted;
            obj.DiscountRate = dto.discount_rate;

            await _context.TblProducts.AddAsync(obj);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var product = await _context.TblProducts.Where(w => w.Id == id).FirstOrDefaultAsync();

            if (product != null)
            {
                _context.TblProducts.Remove(product!);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<ProductDTO> GetById(int id)
        {
            return await _context.TblProducts.Where(w => w.Id == id).Select(s => new ProductDTO
            {
                product_id = s.Id,
                product_name = s.Name,
                product_code = s.Code,
                brand = s.Brand,
                barcode = s.Barcode,
                quantity = (int)s.Quantity,
                unit = s.Unit,
                price = (decimal)s.Price,
                is_discounted = (bool)s.IsDiscounted,
                discount_rate = (decimal)s.DiscountRate
            }).FirstOrDefaultAsync()!;
        }
        public async Task<PaginationDTO<ProductDTO>> List(int page = 1, int page_size = 10, string keyword = "")
        {
            PaginationDTO<ProductDTO> dto = new PaginationDTO<ProductDTO>();

            decimal page_count = _context.TblProducts.Count();
            page = page - 1;

            dto.page_count = (int)Math.Ceiling(page_count / page_size);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                dto.item_list = await _context.TblProducts.Where(w => w.Name!.ToLower().Contains(keyword) || w.Code!.ToLower().Contains(keyword) || w.Barcode!.Contains(keyword))
                    .Select(s => new ProductDTO
                    {
                        product_id = s.Id,
                        product_name = s.Name,
                        product_code = s.Code,
                        brand = s.Brand,
                        barcode = s.Barcode,
                        quantity = (int)s.Quantity,
                        unit = s.Unit,
                        price = (decimal)s.Price,
                        is_discounted = (bool)s.IsDiscounted,
                        discount_rate = (decimal)s.DiscountRate
                    }).Skip(page * page_size).Take(page_size).ToListAsync();
            }
            else
            {
                dto.item_list = await _context.TblProducts.Select(s => new ProductDTO
                {
                    product_id = s.Id,
                    product_name = s.Name,
                    product_code = s.Code,
                    brand = s.Brand,
                    barcode = s.Barcode,
                    quantity = (int)s.Quantity,
                    unit = s.Unit,
                    price = (decimal)s.Price,
                    is_discounted = (bool)s.IsDiscounted,
                    discount_rate = (decimal)s.DiscountRate
                }).Skip(page * page_size).Take(page_size).ToListAsync();
            }


            return dto;
        }
        public async Task Update(ProductDTO dto)
        {
            var product = await _context.TblProducts.Where(w => w.Id == dto.product_id).FirstOrDefaultAsync();

            if (product != null)
            {
                Session session = JsonConvert.DeserializeObject<Session>(_httpContextAccessor.HttpContext!.Session.GetString("UserSession")!)!;

                TblProductHistory product_history = new TblProductHistory();
                product_history.Name = dto.product_name;
                product_history.Code = dto.product_code;
                product_history.Barcode = dto.barcode;
                product_history.Brand = dto.brand;
                product_history.Price = dto.price;
                product_history.Unit = dto.unit;
                product_history.OldQuantity = product.Quantity;
                product_history.AddedQuantity = dto.additional_quantity;
                product_history.NewQuantity = product.Quantity + dto.additional_quantity;
                product_history.IsDiscounted = dto.is_discounted;
                product_history.DiscountRate = dto.discount_rate;
                product_history.DateAdded = DateTime.Now;
                product_history.AddedBy = session.user_id;

                await _context.TblProductHistories.AddAsync(product_history);

                product.Name = dto.product_name;
                product.Code = dto.product_code;
                product.Barcode = dto.barcode;
                product.Brand = dto.brand;
                product.Price = dto.price;
                product.Unit = dto.unit;
                product.Quantity += dto.additional_quantity;
                product.IsDiscounted = dto.is_discounted;
                product.DiscountRate = dto.discount_rate;

                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> ExistingProduct(ProductDTO dto)
        {
            return await _context.TblProducts.AnyAsync(a => a.Name == dto.product_name || a.Code == dto.product_code || a.Barcode == dto.barcode);
        }

        public async Task<IEnumerable<ProductDTO>> Search(string keyword)
        {
            return await _context.TblProducts.Where(w => w.Name.ToLower().Contains(keyword) || w.Code.ToLower().Contains(keyword) || w.Barcode.Contains(keyword))
                         .Select(s => new ProductDTO
                         {
                             product_id = s.Id,
                             product_name = s.Name,
                             product_code = s.Code,
                             brand = s.Brand,
                             barcode = s.Barcode,
                             quantity = (int)s.Quantity,
                             unit = s.Unit,
                             price = (decimal)s.Price,
                             is_discounted = (bool)s.IsDiscounted,
                             discount_rate = (decimal)s.DiscountRate
                         }).Take(5).ToListAsync();
        }
    }
}
