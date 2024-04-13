using Microsoft.AspNetCore.Mvc;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Implementations;
using Sales_Inventory.Repository.Interfaces;

namespace Sales_Inventory.Controllers
{
    [Route("customers")]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerrepository;

        public CustomerController(ICustomerRepository customerrepository)
        {
            _customerrepository = customerrepository;
        }

        [HttpGet("list")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("customer-list")]
        public async Task<IActionResult> CustomerListPartial(int page = 1, string keyword = "")
        {
            PaginationDTO<CustomerDTO> dto = new PaginationDTO<CustomerDTO>();

            dto = await _customerrepository.List(page, 10, keyword);

            ViewBag.Page = page;

            return PartialView("_CustomerListPartial", dto);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CustomerDTO dto)
        {

            if (!ModelState.IsValid)
                return View(dto);

            var response = "Customer Added!";

            try
            {
                var existing_product = await _customerrepository.ExistingCustomer(dto);

                if (existing_product)
                    response = "Customer Already Exist!";
                else
                    await _customerrepository.Create(dto);
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }

            return Json(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(CustomerDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var response = "Customer Updated!";

            try
            {
                await _customerrepository.Update(dto);
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }

            return Json(response);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = "Customer Deleted!";

            try
            {
                await _customerrepository.Delete(id);
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
            CustomerDTO customer = await _customerrepository.GetById(id);

            return Json(customer);
        }
    }
}
