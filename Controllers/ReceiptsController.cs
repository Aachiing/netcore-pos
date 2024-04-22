using Microsoft.AspNetCore.Mvc;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Implementations;
using Sales_Inventory.Repository.Interfaces;

namespace Sales_Inventory.Controllers
{
    [Route("receipts")]
    public class ReceiptsController : Controller
    {
        private readonly IReceiptsRepository _receiptsrepository;

        public ReceiptsController(IReceiptsRepository receiptsrepository)
        {
            _receiptsrepository = receiptsrepository;
        }

        [HttpGet("list")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("receipts-list")]
        public async Task<IActionResult> ReceiptsListPartial(int page = 1, string keyword = "")
        {
            PaginationDTO<ReceiptsDTO> dto = new PaginationDTO<ReceiptsDTO>();

            dto = await _receiptsrepository.List(page, 10, keyword);

            ViewBag.Page = page;

            return PartialView("_ReceiptListPartial", dto);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(ReceiptsDTO dto)
        {
            if (!ModelState.IsValid)
                return Json(string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));

            var response = "Receipt Added!";

            try
            {
                await _receiptsrepository.Create(dto);
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }

            return Json(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(ReceiptsDTO dto)
        {
            if (!ModelState.IsValid)
                return Json(string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));

            var response = "Receipt Updated!";

            try
            {
                await _receiptsrepository.Update(dto);
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
            ReceiptsDTO receipt = await _receiptsrepository.GetById(id);

            return Json(receipt);
        }
    }
}
