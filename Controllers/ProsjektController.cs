using System.Linq;
using Microsoft.AspNetCore.Mvc;
using timeliste.Data;
using timeliste.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace timeliste.Controllers
{
    public class ProsjektController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProsjektController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET
        [Authorize(Roles="Admin,Leder,Regnskap")]
        public async Task<IActionResult> Index()
        {
            var prosjekter = _context.Prosjekt.OrderByDescending(
                p => p.Id).ToList();
            
            return View(prosjekter);
        }
        
        // GET Create
        [HttpGet]
        [Authorize(Roles = "Admin,Regnskap,Leder")]
        public IActionResult Create()
        {
            return View();
        }
        
        // POST Create
        [HttpPost]
        [Authorize(Roles="Admin,Regnskap,Leder")]
        public async Task<IActionResult> Create([Bind("Id, ProsjektNavn, KundeNavn, Info, PeriodeLengde")] Prosjekt prosjekt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prosjekt);
                _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        
        // GET Edit
        [HttpGet]
        [Authorize(Roles = "Admin,Regnskap,Leder")]
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var prosjekt = await _context.Prosjekt.FindAsync(Id);
            return View(prosjekt);
        }
        
        // POST Edit
        [HttpPost]
        [Authorize(Roles = "Admin, Regnskap,Leder")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProsjektNavn,KundeNavn,Info, PeriodeLengde")] Prosjekt prosjekt)
        {
            if (id != prosjekt.Id)
            {
                return NotFound();
            }
            
            // Ser om id er i database
            var ProsjektIDatabase = _context.Prosjekt.Where(
                p => p.Id == id);
            
            if (ModelState.IsValid)
            {
                _context.Update(prosjekt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View();
        }
        // GET prosjekt/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                NotFound();
            }

            var prosjekt = _context.Prosjekt.FirstOrDefault(
                p => p.Id == id);

            if (prosjekt == null)
            {
                return NotFound();
            }

            return View(prosjekt);
        }
    }
}