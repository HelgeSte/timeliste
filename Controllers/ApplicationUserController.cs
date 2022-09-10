using Microsoft.AspNetCore.Mvc;
using timeliste.Models;
using timeliste.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace timeliste.Controllers
{
    public class ApplicationUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public ApplicationUserController(ApplicationDbContext context, UserManager<ApplicationUser> UserManager)
        {
            _context = context;
            this._userManager = UserManager;
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var users = _userManager.Users.OrderByDescending(u => u.Id).ToList();

            return View(users);
        }

        // Get ApplicationUser/EditProfile/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProfile(string id) // string? id eller ApplicationUser
        {
            if (id == null)
            {
                return NotFound();
            }

            //var user = await _userManager.Users.FirstOrDefaultAsync(i => i.Id == id);
            ApplicationUser user = await _userManager.Users.FirstOrDefaultAsync(i => i.Id == id);
            //ApplicationUser currentUser = await _userManager.GetUserAsync(User);

            return View(user);
        }

        // POST : ApplicationUser/EditProfile/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(string id, [Bind("Id, UserName, Email, FirstName, LastName, AnsattNr")] ApplicationUser appuser)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == appuser.Id);

                if (user == null)
                {
                    return NotFound();
                }

                user.UserName = appuser.UserName;
                user.FirstName = appuser.FirstName;
                user.LastName = appuser.LastName;
                user.AnsattNr = appuser.AnsattNr;
                user.Email = appuser.Email;
                
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
                //return View(appuser);
            }
            return View(appuser);
            
        }
        
        private bool UserExists(string id)
        {
            return _userManager.Users.Any(f => f.Id == id);
            //return _context.Users.Any(e => e.Id == id);
        }
    }

    
}