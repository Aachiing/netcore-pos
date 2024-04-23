using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sales_Inventory.Models;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Interfaces;

namespace Sales_Inventory.Repository.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly salesinventory_dbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PaymentRepository(salesinventory_dbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _httpContextAccessor = contextAccessor; 
        }

        public async Task PostCashPayment(OrderHeaderDTO dto)
        {
            Session session = JsonConvert.DeserializeObject<Session>(_httpContextAccessor.HttpContext!.Session.GetString("UserSession")!)!;

            decimal _gross = dto.order_details.Sum(s => s.total_amount);
            decimal vat_rate = (decimal)1.12;

            TblOrderHeader order_header = new TblOrderHeader();
            order_header.OrderNo = dto.order_no;
            order_header.CustomerName = dto.customer_name;
            order_header.DiscountRemarks = dto.remarks;
            order_header.Discount = dto.discount;
            order_header.AmountPaid = dto.amount_paid;
            order_header.Gross = _gross - dto.discount;
            order_header.Net = (_gross - dto.discount) - ((_gross / vat_rate) * (decimal)0.12);
            order_header.Vat = (_gross / vat_rate) * (decimal)0.12;
            order_header.TotalItems = dto.order_details.Sum(s => s.quantity);
            order_header.PaymentType = dto.payment_type;
            order_header.CardNo = dto.card_no;
            order_header.CheckNo = dto.check_no;
            order_header.CheckAmount = dto.check_amount;
            order_header.CheckDate = dto.payment_type == "CHECK" ? dto.check_date : null;
            order_header.TransactionDate = DateTime.Now;
            order_header.CashierId = session.user_id;
            order_header.Cashier = session.full_name;

            await _context.TblOrderHeaders.AddAsync(order_header);
            await _context.SaveChangesAsync();

            if (order_header.Id > 0)
            {
                foreach (var item in dto.order_details)
                {
                    TblOrderDetail order_details = new TblOrderDetail();
                    order_details.OrderNo = order_header.OrderNo;
                    order_details.OrderHeaderId = order_header.Id;
                    order_details.ItemName = item.item_name;
                    order_details.Barcode = item.barcode;
                    order_details.Quantity = item.quantity;
                    order_details.TotalAmount = item.total_amount;
                    order_details.Unit = item.unit;
                    order_details.UnitPrice = item.unit_price;
                    order_details.Discount = item.discount;
                    order_details.DiscountRate = item.discount_rate;

                    await _context.TblOrderDetails.AddAsync(order_details);

                    TblProduct product = await _context.TblProducts.Where(w => w.Id == item.product_id).SingleOrDefaultAsync()!;
                    product.Quantity -= item.quantity;
                }

                await _context.SaveChangesAsync();
            }
        }
        public async Task<ReceivableDTO> PostCreditPayment(OrderHeaderDTO dto)
        {
            Session session = JsonConvert.DeserializeObject<Session>(_httpContextAccessor.HttpContext!.Session.GetString("UserSession")!)!;

            decimal _gross = dto.order_details.Sum(s => s.total_amount);
            decimal vat_rate = (decimal)1.12;

            TblCreditHeader order_header = new TblCreditHeader();
            order_header.OrderNo = dto.order_no;
            order_header.CustomerName = dto.customer_name;
            order_header.AmountPaid = dto.amount_paid;
            order_header.Balance = ((_gross - dto.discount) - dto.amount_paid);
            order_header.DiscountRemarks = dto.remarks;
            order_header.Discount = dto.discount;
            order_header.AmountPaid = dto.amount_paid;
            order_header.Gross = _gross - dto.discount;
            order_header.Net = (_gross - dto.discount) - ((_gross / vat_rate) * (decimal)0.12);
            order_header.Vat = (_gross / vat_rate) * (decimal)0.12;
            order_header.TotalItems = dto.order_details.Sum(s => s.quantity);
            order_header.IsPaid = false;
            order_header.PaymentType = dto.payment_type;
            order_header.CardNo = dto.card_no;
            order_header.CheckNo = dto.check_no;
            order_header.CheckAmount = dto.check_amount;
            order_header.CheckDate = dto.payment_type == "CHECK" ? dto.check_date : null;
            order_header.TransactionDate = DateTime.Now;
            order_header.CashierId = session.user_id;
            order_header.Cashier = session.full_name;

            await _context.TblCreditHeaders.AddAsync(order_header);
            await _context.SaveChangesAsync();

            if (order_header.Id > 0)
            {
                foreach (var item in dto.order_details)
                {
                    TblCreditDetail order_details = new TblCreditDetail();
                    order_details.OrderNo = order_header.OrderNo;
                    order_details.OrderHeaderId = order_header.Id;
                    order_details.ItemName = item.item_name;
                    order_details.Barcode = item.barcode;
                    order_details.Quantity = item.quantity;
                    order_details.TotalAmount = item.total_amount;
                    order_details.Unit = item.unit;
                    order_details.UnitPrice = item.unit_price;
                    order_details.Discount = item.discount;
                    order_details.DiscountRate = item.discount_rate;

                    await _context.TblCreditDetails.AddAsync(order_details);

                    TblProduct product = await _context.TblProducts.Where(w => w.Id == item.product_id).SingleOrDefaultAsync();
                    product.Quantity -= item.quantity;
                }

                await _context.SaveChangesAsync();
            }

            return new ReceivableDTO
            {
                credit_header_id = order_header.Id,
                order_no = order_header.OrderNo,
                customer_name = order_header.CustomerName,
                amount_paid = (decimal)order_header.AmountPaid,
                payment_type = order_header.PaymentType,
                payment_date = order_header.TransactionDate,
                cashier = order_header.Cashier!,
                cashier_id = order_header.CashierId
            };
        }
        public async Task<string> GetOrNo()
        {
            var order_no = _context.TblOrderHeaders.OrderByDescending(ob => ob.Id).Select(s => s.OrderNo).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(order_no))
                order_no = (Convert.ToInt32(order_no) + 1).ToString().PadLeft(8, '0');
            else
                order_no = _context.TblReceipts.OrderByDescending(ob => ob.From).Where(w => w.Type == "OR").Select(s => s.From).FirstOrDefault()!;

            return order_no;
        }
        public async Task<string> GetTRANo()
        {
            var order_no = _context.TblCreditHeaders.OrderByDescending(ob => ob.Id).Select(s => s.OrderNo).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(order_no))
                order_no = (Convert.ToInt32(order_no) + 1).ToString().PadLeft(8, '0');
            else
                order_no = _context.TblReceipts.OrderByDescending(ob => ob.From).Where(w => w.Type == "TRA").Select(s => s.From).FirstOrDefault()!;

            return order_no;
        }
    }
}
