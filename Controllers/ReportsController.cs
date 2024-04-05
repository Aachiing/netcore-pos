using Microsoft.AspNetCore.Mvc;
using Sales_Inventory.Repository.Interfaces;

namespace Sales_Inventory.Controllers
{
    [Route("reports")]
    public class ReportsController : Controller
    {
        private readonly IProductRepository _productrepository;
        private readonly IPaymentRepository _paymentrepository;
        private readonly IReceivableRepository _receivablerepository;
        private readonly IReportRepository _reportrepository;
        private readonly ILogger<HomeController> _logger;

        public ReportsController(ILogger<HomeController> logger, IProductRepository productrepository, IPaymentRepository paymentrepository, IReceivableRepository receivablerepository, IReportRepository reportrepository)
        {
            _logger = logger;
            _productrepository = productrepository;
            _paymentrepository = paymentrepository;
            _receivablerepository = receivablerepository;
            _reportrepository = reportrepository;
        }

        [HttpGet("daily-sales-view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("daily-credit-view")]
        public IActionResult DailyCreditView()
        {
            return View();
        }

        [HttpPost("daily-sales")]
        public async Task<IActionResult> GetDailySalesReport()
        {
            string filePath = string.Empty;

            try
            {
                filePath = await _reportrepository.DailyCashSalesReport();
            }
            catch (Exception ex)
            {
                return Json(new { status = 500, response = ex.Message });
            }

            return Json(new { status = 500, response = filePath });
        }

        [HttpPost("daily-credit")]
        public async Task<IActionResult> GetDailyCreditSalesReport()
        {
            string filePath = string.Empty;

            try
            {
                filePath = await _reportrepository.DailyCreditSalesReport();
            }
            catch (Exception ex)
            {
                return Json(new { status = 500, response = ex.Message });
            }

            return Json(new { status = 500, response = filePath });
        }
    }
}
