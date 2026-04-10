using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using KertKerdes.Data;
using KertKerdes.Models;
using System;
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

        public IActionResult Index(string kereses, int? temakorId, string cimke)
        {
            var kerdesek = _context.Kerdesek
                .Include(k => k.Felhasznalo)
                .Include(k => k.Valaszok)
                .Include(k => k.KerdesCimkek)
                    .ThenInclude(kc => kc.Cimke)
                .Where(k => k.Jovahagyva);

            if (!string.IsNullOrWhiteSpace(kereses))
            {
                var keresettSzoveg = kereses.Trim().ToLower();

                kerdesek = kerdesek.Where(k =>
                    k.Cim.ToLower().Contains(keresettSzoveg) ||
                    k.Leiras.ToLower().Contains(keresettSzoveg));
            }

            if (temakorId.HasValue)
            {
                kerdesek = kerdesek.Where(k => k.TemakorId == temakorId.Value);
            }

            if (!string.IsNullOrWhiteSpace(cimke))
            {
                kerdesek = kerdesek.Where(k =>
                    k.KerdesCimkek.Any(kc =>
                        kc.Cimke != null &&
                        kc.Cimke.Nev == cimke));
            }

            ViewBag.KerdesDb = _context.Kerdesek.Count(k => k.Jovahagyva);
            ViewBag.ValaszDb = _context.Valaszok.Count(v => v.Jovahagyva);

            ViewBag.Temakorok = _context.Temakorok.ToList();
            ViewBag.AktivTemakor = temakorId;
            ViewBag.AktivCimke = cimke;

            ViewBag.TopCimkek = _context.KerdesCimkek
                .Include(kc => kc.Cimke)
                .Where(kc => kc.Cimke != null)
                .GroupBy(kc => kc.Cimke!.Nev)
                .Select(g => new
                {
                    Nev = g.Key,
                    Db = g.Count()
                })
                .OrderByDescending(x => x.Db)
                .Take(12)
                .ToList();

            return View(kerdesek.ToList());
        }

        public IActionResult Letrehozas()
        {
            var felhasznaloId = HttpContext.Session.GetInt32("FelhasznaloId");

            if (felhasznaloId == null)
            {
                TempData["BelepesSzukseges"] = true;
                return RedirectToAction("Bejelentkezes", "Fiok");
            }

            ViewBag.Temakorok = _context.Temakorok.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Letrehozas(Kerdes kerdes, string ujCimkek)
        {
            var felhasznaloId = HttpContext.Session.GetInt32("FelhasznaloId");

            if (felhasznaloId == null)
            {
                TempData["BelepesSzukseges"] = true;
                return RedirectToAction("Bejelentkezes", "Fiok");
            }

            if (string.IsNullOrWhiteSpace(kerdes.Cim) || string.IsNullOrWhiteSpace(kerdes.Leiras))
            {
                ViewBag.Temakorok = _context.Temakorok.ToList();
                ModelState.AddModelError("", "A kérdés címe és leírása kötelező.");
                return View(kerdes);
            }

            kerdes.Jovahagyva = false;
            kerdes.Szavazat = 0;
            kerdes.FelhasznaloId = felhasznaloId.Value;
            kerdes.Datum = DateTime.Now;

            _context.Kerdesek.Add(kerdes);
            _context.SaveChanges();

            if (!string.IsNullOrWhiteSpace(ujCimkek))
            {
                var egyediCimkek = ujCimkek
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(c => c.Trim().ToLower())
                    .Distinct()
                    .ToList();

                foreach (var cimkeNev in egyediCimkek)
                {
                    var letezoCimke = _context.Cimkek.FirstOrDefault(c => c.Nev.ToLower() == cimkeNev);

                    if (letezoCimke == null)
                    {
                        letezoCimke = new Cimke
                        {
                            Nev = cimkeNev
                        };

                        _context.Cimkek.Add(letezoCimke);
                        _context.SaveChanges();
                    }

                    var kapcsolatLetezik = _context.KerdesCimkek.Any(kc =>
                        kc.KerdesId == kerdes.Id &&
                        kc.CimkeId == letezoCimke.Id);

                    if (!kapcsolatLetezik)
                    {
                        _context.KerdesCimkek.Add(new KerdesCimke
                        {
                            KerdesId = kerdes.Id,
                            CimkeId = letezoCimke.Id
                        });
                    }
                }

                _context.SaveChanges();
            }

            TempData["KerdesSikeres"] = true;

            return RedirectToAction("Index");
        }

        public IActionResult Reszletek(int id)
        {
            var kerdes = _context.Kerdesek
                .Include(k => k.Felhasznalo)
                .Include(k => k.Temakor)
                .Include(k => k.KerdesCimkek)
                    .ThenInclude(kc => kc.Cimke)
                .Include(k => k.Valaszok.Where(v => v.Jovahagyva))
                    .ThenInclude(v => v.Felhasznalo)
                .FirstOrDefault(k => k.Id == id && k.Jovahagyva);

            if (kerdes == null)
            {
                return NotFound();
            }

            return View(kerdes);
        }
    }
}