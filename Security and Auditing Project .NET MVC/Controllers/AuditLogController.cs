using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Security_and_Auditing_Project_.NET_MVC.Data;

namespace Security_and_Auditing_Project_.NET_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuditLogController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AuditLogController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var getAuditLogs = _db.AuditLogs.Include(x => x.User)
                .ThenInclude(x => x.Gender)
                .ToList();
            return View(getAuditLogs);
        }
    }
}
