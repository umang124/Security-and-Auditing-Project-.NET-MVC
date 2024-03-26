using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Security_and_Auditing_Project_.NET_MVC.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
