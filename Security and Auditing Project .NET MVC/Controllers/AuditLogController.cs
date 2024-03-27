using Microsoft.AspNetCore.Mvc;
using Security_and_Auditing_Project_.NET_MVC.Models.VM;

namespace Security_and_Auditing_Project_.NET_MVC.Controllers
{
    public class AuditLogController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
