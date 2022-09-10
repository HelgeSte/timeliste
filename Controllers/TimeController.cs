using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using timeliste.Data;
using timeliste.Models;
using timeliste.Utilities;

namespace timeliste.Controllers {
    [Authorize]
    public class TimeController : Controller
    {
        private readonly ILogger<TimeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly UserManager<ApplicationUser> _userMan;


        public TimeController(ILogger<TimeController>  logger, ApplicationDbContext context, IWebHostEnvironment hostEnv, UserManager<ApplicationUser> userMan)
        {
            _logger = logger;
            _context = context;
            _hostEnv = hostEnv;
            _userMan = userMan;
        }

        [HttpGet]
        public IActionResult Export() {
            List<Prosjekt> prosjekter = _context.Prosjekt.ToListAsync().Result;
            ViewBag.Prosjekter = prosjekter;

            return View(_context.Time.ToListAsync().Result);
		}

        [HttpPost]
        public async Task<IActionResult> Export(IFormCollection collection) {
            var prosjektId = Int32.Parse(collection["Prosjekt"]);
            var fraDato = DateTime.Parse(collection["fraDato"]);

            var prosjekt = await _context.Prosjekt.FindAsync(prosjektId);

            var startDato = fraDato.AddDays(-prosjekt.PeriodeLengde);

            var generator = new CSVGenerator(_logger, _hostEnv);
            List<Time> timer;

            if(prosjektId == 0) {
                timer = await _context.Time.Where(t => t.Start.Date >= startDato.Date && t.Slutt.Date <= fraDato.Date)
                    .ToListAsync();
			} else {
                timer = await _context.Time.Where(p => p.ProsjektId == prosjektId)
                    .Where(t => t.Start.Date >= startDato.Date && t.Slutt.Date <= fraDato.Date)
                    .ToListAsync();
			}
            
            generator.GenerateFile(timer, prosjektId);

            return RedirectToAction(nameof(Export));
		}

        [HttpPost]
        public async Task<IActionResult> Index(IFormCollection collection) {
            var prosjektId = Int32.Parse(collection["Prosjekt"]);
            
            UInt16 viewAlle;
            var success = UInt16.TryParse(collection["viewAlle"], out viewAlle);
            
            List<Prosjekt> prosjekter = await _context.Prosjekt.ToListAsync();
            ViewBag.Prosjekter = prosjekter;

            var user = await _userMan.GetUserAsync(User);

            List<Time> timer;

            if(viewAlle == 1) {
                if(prosjektId == 0) {
                    timer = await _context.Time.ToListAsync();
                } else {
                    timer = await _context.Time.Where(p => p.ProsjektId == prosjektId).ToListAsync();
                }
            } else {
                if(prosjektId == 0) {
                    timer = await _context.Time.Where(t => t.AnsattNr == user.AnsattNr)
                                    .ToListAsync();
                } else {
                    timer = await _context.Time.Where(p => p.ProsjektId == prosjektId)
                                    .Where(t => t.AnsattNr == user.AnsattNr)
                                    .ToListAsync();
                }
            }
            return View(timer);
        }


        // GET: Time
        [HttpGet]
        public async Task<IActionResult> Index() {
            List<Prosjekt> prosjekter = _context.Prosjekt.ToListAsync().Result;
            ViewBag.Prosjekter = prosjekter;

            var user = await _userMan.GetUserAsync(User);

            return View(await _context.Time.Where(t => t.AnsattNr == user.AnsattNr)
                .ToListAsync());
        }

        // GET: Time/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var time = await _context.Time
                .FirstOrDefaultAsync(m => m.Id == id);
            if (time == null)
            {
                return NotFound();
            }

            return View(time);
        }

        // GET: Time/Create
        public IActionResult Create()
        {
            List<Prosjekt> prosjekter = _context.Prosjekt.ToListAsync().Result;
            ViewBag.Prosjekter = prosjekter;

            return View();
        }

        // POST: Time/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AnsattNr,ProsjektId,Start,Slutt,Kommentar")] Time time)
        {
            if (ModelState.IsValid)
            {
                var user = await _userMan.GetUserAsync(User);

                time.AnsattNr = user.AnsattNr;

                double diff = (time.Slutt - time.Start).TotalHours;
                time.Timer = diff;
                _context.Add(time);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(time);
        }

        // GET: Time/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var time = await _context.Time.FindAsync(id);
            if (time == null)
            {
                return NotFound();
            }
            return View(time);
        }

        // POST: Time/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AnsattNr,ProsjektId,Start,Slutt,Kommentar")] Time time)
        {
            if (id != time.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    double diff = (time.Slutt - time.Start).TotalHours;
                    time.Timer = diff;
                    _context.Update(time);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeExists(time.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(time);
        }

        // GET: Time/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var time = await _context.Time
                .FirstOrDefaultAsync(m => m.Id == id);
            if (time == null)
            {
                return NotFound();
            }

            return View(time);
        }

        // POST: Time/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var time = await _context.Time.FindAsync(id);
            _context.Time.Remove(time);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimeExists(int id)
        {
            return _context.Time.Any(e => e.Id == id);
        }
    }
}
