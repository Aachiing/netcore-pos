﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Interfaces;

namespace Sales_Inventory.Controllers
{
    [Route("orders")]
    public class OrderController : Controller
    {
        private readonly IProductRepository _productrepository;
        private readonly IPaymentRepository _paymentrepository;
        private readonly IReceivableRepository _receivablerepository;
        private readonly ILogger<HomeController> _logger;

        public OrderController(ILogger<HomeController> logger, IProductRepository productrepository, IPaymentRepository paymentrepository, IReceivableRepository receivablerepository)
        {
            _logger = logger;
            _productrepository = productrepository;
            _paymentrepository = paymentrepository;
            _receivablerepository = receivablerepository;
        }

        [Route("list")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("dropdown-list-product/{keyword}")]
        public async Task<IActionResult> ListProduct(string keyword)
        {
            ProductDTO product = new ProductDTO();
            product.list = await _productrepository.Search(keyword);

            var dropdownList = product.list.Select(s => new
            {
                label = $"{s.product_name} (Available: {s.quantity} pcs || Price: Php {s.price})",
                val = s.product_id
            });

            return Json(JsonConvert.SerializeObject(product.list, Formatting.Indented));
        }

        [HttpPost("void-item/{userName}/{password}/{itemId}")]
        public async Task<IActionResult> VoidItem(string userName, string password, int itemId)
        {
            if (userName == "admin" && password == "admin")
            {
                ProductDTO product = await _productrepository.GetById(itemId);

                return Json(new { canVoid = true, obj = product });
            }

            return Json(new { canVoid = false });
        }

        [HttpPost("pay")]
        public async Task<IActionResult> PostPayment(OrderHeaderDTO dto)
        {
            try
            {
                if (dto.transaction_type == "CREDIT")
                {
                    ReceivableDTO receivable = await _paymentrepository.PostCreditPayment(dto);

                    if (receivable != null)
                        await _receivablerepository.AddPaymentHistory(receivable);
                }
                else
                {
                    await _paymentrepository.PostCashPayment(dto);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = 500, msg = ex.Message });
            }

            return Json(new { status = 200, msg = "Order Processed!" });
        }
    }
}
