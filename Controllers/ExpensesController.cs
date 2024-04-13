using Microsoft.AspNetCore.Mvc;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Implementations;
using Sales_Inventory.Repository.Interfaces;

namespace Sales_Inventory.Controllers
{
    [Route("expenses")]
    public class ExpensesController : Controller
    {
        private readonly IExpensesRepository _expensesrepository;

        public ExpensesController(IExpensesRepository expensesrepository)
        {
            _expensesrepository = expensesrepository;
        }

        [HttpGet("list")]
        public async Task<IActionResult> Index(int page = 1)
        {
            PaginationDTO<ExpensesDTO> dto = new PaginationDTO<ExpensesDTO>();

            dto = await _expensesrepository.List(page);

            ViewBag.Page = page;

            return View(dto);
        }

        [HttpGet("expenses-list-body")]
        public async Task<IActionResult> ExpensesListPartial(int page = 1, string keyword = "")
        {
            PaginationDTO<ExpensesDTO> dto = new PaginationDTO<ExpensesDTO>();

            dto = await _expensesrepository.List(page, 10, keyword);

            ViewBag.Page = page;

            return PartialView("_ExpensesListPartial", dto);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(ExpensesDTO dto)
        {

            if (!ModelState.IsValid)
                return View(dto);

            var response = "Expense Added!";

            try
            {
                await _expensesrepository.Create(dto);
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }

            return Json(response);
        }
    }
}
