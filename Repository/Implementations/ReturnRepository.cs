using Microsoft.EntityFrameworkCore;
using Sales_Inventory.Models;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Interfaces;

namespace Sales_Inventory.Repository.Implementations
{
    public class ReturnRepository : IReturnRepository
    {
        private readonly salesinventory_dbContext _context;

        public ReturnRepository(salesinventory_dbContext context)
        {
            _context = context;

        }
        public async Task<List<OrderDetailsDTO>> LoadOrderDetails(string keyword = "")
        {
            List<OrderDetailsDTO> lstOrderDetails = new List<OrderDetailsDTO>();

            lstOrderDetails = await _context.TblOrderDetails
                                          .Where(w => w.OrderNo == keyword)
                                          .Select(s => new OrderDetailsDTO
                                          {
                                              order_id = s.Id,
                                              order_no = s.OrderNo,
                                              item_name = s.ItemName,
                                              barcode = s.Barcode,
                                              quantity = s.Quantity,
                                              unit_price = s.UnitPrice,
                                              total_amount = s.TotalAmount,
                                              return_qty = s.ReturnedQty == null ? 0 : (int)s.ReturnedQty,
                                              returned_date = s.ReturnedDate == null ? "N/A" : Convert.ToDateTime(s.ReturnedDate).ToString("mm/dd/yyyy"),
                                              customer_name = _context.TblOrderHeaders.Where(w => w.OrderNo == keyword).Select(s => s.CustomerName).FirstOrDefault()!
                                          }).ToListAsync();
            return lstOrderDetails;
        }
    }
}
