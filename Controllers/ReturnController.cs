using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Interfaces;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Sales_Inventory.Controllers
{
    [Route("returns")]
    public class ReturnController : Controller
    {
        private readonly IReturnRepository _returnrepository;

        public ReturnController(IReturnRepository returnrepository)
        {
            _returnrepository = returnrepository;
        }

        [HttpGet("list")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("partial-cash-transactions")]
        public async Task<IActionResult> PartialCashTransactions(string keyword = "")
        {
            List<OrderDetailsDTO> lstOrderDetails = new List<OrderDetailsDTO>();

            lstOrderDetails = await _returnrepository.LoadOrderDetails("00000058");

            return PartialView("_CashTransactionsPartial", lstOrderDetails);
        }
    }
}
