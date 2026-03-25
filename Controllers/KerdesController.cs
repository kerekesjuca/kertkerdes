using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KertKerdes.Data;
using KertKerdes.Models;

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
            {
                kerdesek = kerdesek.Where(k =>
                    k.Cim.Contains(kereses) ||
                    k.Leiras.Contains(kereses));
            }

            ViewBag.KerdesDb = _context.Kerdesek.Count(k => k.Jovahagyva);
            ViewBag.ValaszDb = _context.Valaszok.Count(v => v.Jovahagyva);

            ViewBag.Temakorok = _context.Temakorok.ToList();

            return View(kerdesek.ToList());
        }

        public IActionResult Letrehozas()
        {
            ViewBag.Temakorok = _context.Temakorok.ToList();

            ViewBag.Cimkek = _context.Cimkek.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Letrehozas(Kerdes kerdes, List<int> kivalasztottCimkek)
        {
            kerdes.Jovahagyva = false;
            kerdes.Szavazat = 0;
            kerdes.FelhasznaloId = 1;
            kerdes.Datum = DateTime.Now;

            _context.Kerdesek.Add(kerdes);
            _context.SaveChanges();

            foreach (var cimkeId in kivalasztottCimkek)
            {
                _context.KerdesCimkek.Add(new KerdesCimke
                {
                    KerdesId = kerdes.Id,
                    CimkeId = cimkeId
                });
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Reszletek(int id)
        {
            var kerdes = _context.Kerdesek
                .Include(k => k.Valaszok)
                .FirstOrDefault(k => k.Id == id);

            if (kerdes == null)
            {
                return NotFound();
            }

            return View(kerdes);
        }
    }
}