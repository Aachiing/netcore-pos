using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Sales_Inventory.DAL;
using Sales_Inventory.Models;
using Sales_Inventory.Models.DTO;

namespace Sales_Inventory.Controllers
{
    [Route("dashboard")]
    public class DashboardController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly DashboardDAL _dashboardtDAL;

        public DashboardController(IConfiguration configuration)
        {
            _configuration = configuration;
            _dashboardtDAL = new DashboardDAL(_configuration);
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            DashboardDTO dto = new DashboardDTO();
            dto.monthly_revenue = _dashboardtDAL.MonthlyDashboardReport();
            dto.daily_revenue = _dashboardtDAL.DailyDashboardReport();

            return View(dto);
        }
    }
}
