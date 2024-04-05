using Microsoft.EntityFrameworkCore;
using Sales_Inventory.Models;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Interfaces;

namespace Sales_Inventory.Repository.Implementations
{
    public class ReceivableRepository : IReceivableRepository
    {
        private readonly salesinventory_dbContext _context;
        public ReceivableRepository(salesinventory_dbContext context) { _context = context; }

        public async Task AddPaymentHistory(ReceivableDTO dto)
        {
            TblReceivable tblReceivable = new TblReceivable();
            tblReceivable.CreditHeaderId = dto.credit_header_id;
            tblReceivable.OrderNo = await _context.TblCreditHeaders.Where(w => w.Id == dto.credit_header_id).Select(s => s.OrderNo).SingleOrDefaultAsync();
            tblReceivable.CustomerName = await _context.TblCreditHeaders.Where(w => w.Id == dto.credit_header_id).Select(s => s.CustomerName).SingleOrDefaultAsync();
            tblReceivable.AmountPaid = dto.amount_paid;
            tblReceivable.PaymentDate = DateTime.Now;
            tblReceivable.PaymentType = dto.payment_type;
            tblReceivable.Cashier = dto.cashier == null ? "N/A" : dto.cashier;
            tblReceivable.CashierId = dto.cashier_id;

            await _context.TblReceivables.AddAsync(tblReceivable);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderHeaderDTO>> List()
        {
            return await _context.TblCreditHeaders.Where(w => w.IsPaid == false)
                        .Select(s => new OrderHeaderDTO
                        {
                            order_id = s.Id,
                            order_no = s.OrderNo,
                            customer_name = s.CustomerName!,
                            total_items = s.TotalItems,
                            gross = s.Gross,
                            amount_paid = (decimal)s.AmountPaid!,
                            balance = (decimal)s.Balance!,
                        }).ToListAsync();
        }

        public async Task<IEnumerable<OrderDetailsDTO>> OrderDetails(int order_id)
        {
            return await _context.TblCreditDetails.Where(w => w.OrderHeaderId == order_id)
                       .Select(s => new OrderDetailsDTO
                       {
                           item_name = s.ItemName,
                           barcode = s.Barcode,
                           quantity = s.Quantity,
                           unit_price = s.UnitPrice,
                           total_amount = s.TotalAmount,
                       }).ToListAsync();
        }

        public async Task<IEnumerable<ReceivableDTO>> PaymentHistory(int order_id)
        {
            return await _context.TblReceivables.Where(w => w.CreditHeaderId == order_id)
                        .Select(s => new ReceivableDTO
                        {
                            id = s.Id,
                            credit_header_id = s.CreditHeaderId,
                            order_no = s.OrderNo,
                            customer_name = s.CustomerName,
                            amount_paid = s.AmountPaid,
                            payment_type = s.PaymentType,
                            payment_date = s.PaymentDate,
                            cashier = s.Cashier,
                            cashier_id = s.CashierId
                        }).ToListAsync();
        }

        public async Task UpdatePayment(ReceivableDTO dto)
        {
            var credit_header = await _context.TblCreditHeaders.Where(w => w.Id == dto.credit_header_id).FirstOrDefaultAsync();

            if (credit_header != null)
            {
                credit_header.AmountPaid += dto.amount_paid;
                credit_header.Balance -= dto.amount_paid;

                await _context.SaveChangesAsync();
            }
        }
    }
}
