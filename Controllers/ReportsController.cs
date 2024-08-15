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

        [HttpGet("daily-expenses-view")]
        public IActionResult DailyExpensesView()
        {
            return View();
        }

        [HttpGet("inventory-history-view")]
        public IActionResult InventoryHistoryView()
        {
            return View();
        }

        [HttpGet("daily-sales/{dateFrom}/{dateTo}")]
        public async Task<IActionResult> GetDailySalesReport(DateTime dateFrom, DateTime dateTo)
        {
            string filePath = string.Empty;

            try
            {
                filePath = await _reportrepository.DailyCashSalesReport(dateFrom, dateTo);
            }
            catch (Exception ex)
            {
                return Json(new { status = 500, response = ex.Message });
            }

            return Json(new { status = 500, response = filePath });
        }

        [HttpGet("daily-credit/{dateFrom}/{dateTo}")]
        public async Task<IActionResult> GetDailyCreditSalesReport(DateTime dateFrom, DateTime dateTo)
        {
            string filePath = string.Empty;

            try
            {
                filePath = await _reportrepository.DailyCreditSalesReport(dateFrom, dateTo);
            }
            catch (Exception ex)
            {
                return Json(new { status = 500, response = ex.Message });
            }

            return Json(new { status = 500, response = filePath });
        }

        [HttpGet("daily-expenses/{dateFrom}/{dateTo}")]
        public async Task<IActionResult> GetDailyExpensesReport(DateTime dateFrom, DateTime dateTo)
        {
            string filePath = string.Empty;

            try
            {
                filePath = await _reportrepository.DailyExpensesReport(dateFrom, dateTo);
            }
            catch (Exception ex)
            {
                return Json(new { status = 500, response = ex.Message });
            }

            return Json(new { status = 500, response = filePath });
        }

        [HttpGet("inventory-history/{dateFrom}/{dateTo}")]
        public async Task<IActionResult> GetInventoryHistory(DateTime dateFrom, DateTime dateTo)
        {
            string filePath = string.Empty;

            try
            {
                filePath = await _reportrepository.InventoryHistoryReport(dateFrom, dateTo);
            }
            catch (Exception ex)
            {
                return Json(new { status = 500, response = ex.Message });
            }

            return Json(new { status = 500, response = filePath });
        }
    }
}
