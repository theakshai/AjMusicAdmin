using Microsoft.AspNetCore.Mvc;

namespace AjMusicAdmin.Controllers
{
    public class UsersController : Controller
    {
        [Route("admin/users")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
