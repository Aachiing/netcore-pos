using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sales_Inventory.Models;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Interfaces;

namespace Sales_Inventory.Controllers
{
    [Route("users")]
    public class LoginController : Controller
    {
        private readonly IUserRepository _userrepository;

        public LoginController(IUserRepository userrepository)
        {
            _userrepository = userrepository;
        }


        [HttpGet("login")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Index(UserDTO dto)
        {
            if (!ModelState.IsValid)
                return View();

            UserDTO user = await _userrepository.Login(dto);

            if (user != null)
            {
                Session session = new Session()
                {
                    user_id = user.id,
                    full_name = user.full_name,
                    user_type = user.user_type
                };

                HttpContext.Session.SetString("UserSession", JsonConvert.SerializeObject(session));

                var current_session = JsonConvert.DeserializeObject<Session>(HttpContext.Session.GetString("UserSession")!);

                return RedirectToAction("list", "orders");
            }
            else
            {
                TempData["AlertMessage"] = "user not found";
                return View();
            }
        }
    }
}
