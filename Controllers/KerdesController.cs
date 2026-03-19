using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KertKerdes.Data;
using System.Linq;

namespace KertKerdes.Controllers
{
    public class KerdesController : Controller
    {
        private readonly AppDbContext _context;

        public KerdesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string kereses)
        {
            var kerdesek = _context.Kerdesek
                .Include(k => k.Valaszok)
                .Where(k => k.Jovahagyva);

            if (!string.IsNullOrEmpty(kereses))
                kerdesek = kerdesek.Where(k => k.Cim.Contains(kereses));

            ViewBag.KerdesDb = _context.Kerdesek.Count(k => k.Jovahagyva);
            ViewBag.ValaszDb = _context.Valaszok.Count(v => v.Jovahagyva);

            return View(kerdesek.ToList());
        }

        public IActionResult Reszletek(int id)
        {
            var k = _context.Kerdesek
                .Include(x => x.Valaszok)
                .FirstOrDefault(x => x.Id == id);

            return View(k);
        }

        public IActionResult Letrehozas() => View();

        [HttpPost]
        public IActionResult Letrehozas(Models.Kerdes k)
        {
            k.Szerzo = "felhasznalo";
            k.Jovahagyva = false;

            _context.Kerdesek.Add(k);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}