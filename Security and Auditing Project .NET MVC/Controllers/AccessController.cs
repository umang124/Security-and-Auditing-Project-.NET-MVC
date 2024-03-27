using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Security_and_Auditing_Project_.NET_MVC.Data;
using Security_and_Auditing_Project_.NET_MVC.Models;
using Security_and_Auditing_Project_.NET_MVC.Models.VM;
using System.Security.Claims;

namespace Security_and_Auditing_Project_.NET_MVC.Controllers
{
    public class AccessController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AccessController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult Register()
        {
            IEnumerable<SelectListItem> GenderList = _db.Genders.ToList().
                                                    Select(x => new SelectListItem
                                                    {
                                                        Text = x.Name,
                                                        Value = x.Id.ToString()
                                                    });

            IEnumerable<SelectListItem> RolesList = _db.Roles.ToList().
                                                    Select(x => new SelectListItem
                                                    {
                                                        Text = x.Name,
                                                        Value = x.Id.ToString()
                                                    });

            ViewBag.GenderList = GenderList;
            ViewBag.RolesList = RolesList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {

            if (ModelState.IsValid)
            {
                if (_db.Users.Any(x => x.UserName == registerVM.UserName))
                {
                    ModelState.AddModelError("UserName", "UserName already exists");
                    return View();
                }
                if (_db.Users.Any(x => x.Email == registerVM.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View();
                }


                Models.User userModel = new Models.User();
                userModel.GenderId = registerVM.GenderId;
                userModel.UserName = registerVM.UserName;
                userModel.Email = registerVM.Email;
                userModel.Password = registerVM.Password;
                _db.Users.Add(userModel);
                _db.SaveChanges();

                UserRole userRoleModel = new UserRole();
                userRoleModel.UserId = userModel.Id;
                userRoleModel.RoleId = registerVM.RoleId;
                _db.UserRoles.Add(userRoleModel);
                _db.SaveChanges();

                AuditLog auditLog = new AuditLog();
                auditLog.Action = "Registered";
                auditLog.Timestamp = DateTime.Now;
                auditLog.UserId = userModel.Id;
                _db.AuditLogs.Add(auditLog);
                _db.SaveChanges();

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, userModel.Id.ToString()),
                    new Claim(ClaimTypes.Email, userModel.Email),
                    new Claim(ClaimTypes.Role, _db.Roles.FirstOrDefault(x => x.Id == registerVM.RoleId).Name)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Notes");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                if (_db.Users.Any(x => x.Email == loginVM.Email && x.Password == loginVM.Password))
                {
                    var userId = _db.Users.FirstOrDefault(x => x.Email == loginVM.Email).Id;
                    var userRoleId = _db.UserRoles.FirstOrDefault(x => x.UserId == userId).RoleId;

                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString()), 
                        new Claim(ClaimTypes.Email, loginVM.Email), 
                        new Claim(ClaimTypes.Role, _db.Roles.FirstOrDefault(x => x.Id == userRoleId).Name)
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                    new ClaimsPrincipal(claimsIdentity), properties);


                    AuditLog auditLog = new AuditLog();
                    auditLog.Action = "Logged In";
                    auditLog.Timestamp = DateTime.Now;
                    auditLog.UserId = userId;
                    _db.AuditLogs.Add(auditLog);
                    _db.SaveChanges();

                    return RedirectToAction("Index", "Notes");
                }
                ModelState.AddModelError("Email", "Email or Password is incorrect");
                return View();
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            var userIdClaim = claimsIdentity?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            AuditLog auditLog = new AuditLog();
            auditLog.Action = "Logged Out";
            auditLog.Timestamp = DateTime.Now;
            auditLog.UserId = Convert.ToInt32(userIdClaim.Value);
            _db.AuditLogs.Add(auditLog);
            _db.SaveChanges();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
