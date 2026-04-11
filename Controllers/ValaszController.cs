using KertKerdes.Data;
using KertKerdes.Helpers;
using KertKerdes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KertKerdes.Controllers
{
    public class ValaszController : Controller
    {
        private readonly AppDbContext _context;

        public ValaszController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Letrehozas(Valasz valasz)
        {
            var felhasznaloId = HttpContext.Session.GetInt32("FelhasznaloId");

            if (felhasznaloId == null)
            {
                TempData["BelepesSzukseges"] = true;
                return RedirectToAction("Bejelentkezes", "Fiok");
            }

            if (string.IsNullOrWhiteSpace(valasz.Szoveg))
            {
                TempData["ValaszHiba"] = "A válasz szövege nem lehet üres.";
                TempData["ValaszSzoveg"] = valasz.Szoveg;

                return RedirectToAction("Reszletek", "Kerdes", new { id = valasz.KerdesId });
            }

            if (CimkeModeraloHelper.TiltottCimke(valasz.Szoveg))
            {
                TempData["ValaszHiba"] = "A szöveg trágár kifejezést tartalmaz, módosítsd.";
                TempData["ValaszSzoveg"] = valasz.Szoveg;

                return RedirectToAction("Reszletek", "Kerdes", new { id = valasz.KerdesId });
            }

            valasz.Jovahagyva = false;
            valasz.Elfogadott = false;
            valasz.Szavazat = 0;
            valasz.FelhasznaloId = felhasznaloId.Value;
            valasz.Datum = DateTime.Now;

            _context.Valaszok.Add(valasz);
            _context.SaveChanges();

            TempData["ValaszBekuldve"] = true;

            return RedirectToAction("Reszletek", "Kerdes", new { id = valasz.KerdesId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Elfogad(int id)
        {
            var valasz = _context.Valaszok.FirstOrDefault(v => v.Id == id);

            if (valasz == null)
                return NotFound();

            if (!valasz.Jovahagyva)
                return RedirectToAction("Reszletek", "Kerdes", new { id = valasz.KerdesId });

            if (valasz.Elfogadott)
                return RedirectToAction("Reszletek", "Kerdes", new { id = valasz.KerdesId });

            var kerdes = _context.Kerdesek.FirstOrDefault(k => k.Id == valasz.KerdesId);

            if (kerdes == null)
                return NotFound();

            var aktualisFelhasznaloId = HttpContext.Session.GetInt32("FelhasznaloId");

            if (aktualisFelhasznaloId == null || kerdes.FelhasznaloId != aktualisFelhasznaloId.Value)
                return Unauthorized();

            var korabbanElfogadottValaszok = _context.Valaszok
                .Where(v => v.KerdesId == kerdes.Id && v.Elfogadott)
                .ToList();

            foreach (var regiValasz in korabbanElfogadottValaszok)
            {
                regiValasz.Elfogadott = false;
            }

            valasz.Elfogadott = true;

            _context.SaveChanges();

            TempData["ValaszElfogadva"] = true;

            return RedirectToAction("Reszletek", "Kerdes", new { id = kerdes.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ElfogadasVisszavon(int id)
        {
            var valasz = _context.Valaszok.FirstOrDefault(v => v.Id == id);

            if (valasz == null)
                return NotFound();

            if (!valasz.Jovahagyva)
                return RedirectToAction("Reszletek", "Kerdes", new { id = valasz.KerdesId });

            var kerdes = _context.Kerdesek.FirstOrDefault(k => k.Id == valasz.KerdesId);

            if (kerdes == null)
                return NotFound();

            var aktualisFelhasznaloId = HttpContext.Session.GetInt32("FelhasznaloId");

            if (aktualisFelhasznaloId == null || kerdes.FelhasznaloId != aktualisFelhasznaloId.Value)
                return Unauthorized();

            valasz.Elfogadott = false;

            _context.SaveChanges();

            TempData["ValaszElfogadasTorolve"] = true;

            return RedirectToAction("Reszletek", "Kerdes", new { id = kerdes.Id });
        }
    }
}