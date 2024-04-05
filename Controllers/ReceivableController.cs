using Microsoft.AspNetCore.Mvc;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Implementations;
using Sales_Inventory.Repository.Interfaces;

namespace Sales_Inventory.Controllers
{
    [Route("acct-receivables")]
    public class ReceivableController : Controller
    {
        private readonly IReceivableRepository _receivablepository;
        public ReceivableController(IReceivableRepository receivablepository)
        {
            _receivablepository = receivablepository;
        }

        [HttpGet("list")]
        public async Task<IActionResult> Index()
        {
            var receivables = await _receivablepository.List();

            return View(receivables);
        }

        [HttpGet("payment-history/{credit_header_id}")]
        public async Task<IActionResult> PartialPaymentHistory(int credit_header_id)
        {
            IEnumerable<ReceivableDTO> receivables = await _receivablepository.PaymentHistory(credit_header_id);

            return PartialView("_paymentHistory", receivables);
        }

        [HttpGet("order-details/{credit_header_id}")]
        public async Task<IActionResult> PartialOrderDetails(int credit_header_id)
        {
            IEnumerable<OrderDetailsDTO> order_details = await _receivablepository.OrderDetails(credit_header_id);

            return PartialView("_orderDetails", order_details);
        }

        [HttpPost("pay")]
        public async Task<IActionResult> AddPayment(ReceivableDTO dto)
        {
            try
            {
                await _receivablepository.AddPaymentHistory(dto);
                await _receivablepository.UpdatePayment(dto);

            }
            catch (Exception ex)
            {
                return Json(new { status = 500, msg = ex.Message });
            }

            return Json(new { status = 200, msg = "Payment Added!" });
        }
    }
}
