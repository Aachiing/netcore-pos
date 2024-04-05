using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sales_Inventory.Models;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Interfaces;
using System.Diagnostics;
using System.Drawing;
using System.Reflection.Emit;

namespace Sales_Inventory.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IProductRepository _productrepository;
        private readonly IPaymentRepository _paymentrepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IProductRepository productrepository, IPaymentRepository paymentrepository)
        {
            _logger = logger;
            _productrepository = productrepository;
            _paymentrepository = paymentrepository;
        }

        public IActionResult Index()
        {
            return RedirectToAction("login", "users");
        }


    }
}
