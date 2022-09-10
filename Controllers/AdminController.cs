using Microsoft.AspNetCore.Mvc;
using timeliste.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace timeliste.Controllers
{
    public class AdminController : Controller
    {
        // Variabel and constructor for user management
        private readonly UserManager<ApplicationUser> _UserManager;
        public AdminController(UserManager<ApplicationUser> UserManager)
        {
            this._UserManager = UserManager;
        }
        // GET Users
        [Authorize(Roles="Admin")]
        public IActionResult Userlist()
        {
            var users = _UserManager.Users;
            return View(users);
        }

        // GET
        [Authorize(Roles="Admin")]
        public IActionResult Index()
        {
            return View();
        }
        
        
        
    }
}