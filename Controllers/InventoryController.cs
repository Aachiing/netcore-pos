using Microsoft.AspNetCore.Mvc;
using Sales_Inventory.BLL;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Interfaces;

namespace Sales_Inventory.Controllers
{
    [Route("inventory")]
    public class InventoryController : Controller
    {
        private readonly IProductRepository _productrepository;

        public InventoryController(IProductRepository productrepository)
        {
            _productrepository = productrepository;
        }

        [HttpGet("list")]
        public async Task<IActionResult> Index(int page = 1)
        {
            PaginationDTO<ProductDTO> dto = new PaginationDTO<ProductDTO>();

            dto = await _productrepository.List(page);

            ViewBag.Page = page;
            return View(dto);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(ProductDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var response = "Product Added!";

            try
            {
                var existing_product = await _productrepository.ExistingProduct(dto);

                if (existing_product)
                    response = "Product Already Exist!";
                else
                    await _productrepository.Create(dto);
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }

            return Json(response);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetByid(int id)
        {
            ProductDTO product = await _productrepository.GetById(id);

            return Json(product);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(ProductDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var response = "Product Updated!";

            try
            {
                await _productrepository.Update(dto);
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }

            return Json(response);
        }
    }
}
