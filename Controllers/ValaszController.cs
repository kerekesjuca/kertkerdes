using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using KertKerdes.Data;
using KertKerdes.Models;
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

        public IActionResult Elfogad(int id)
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

            if (valasz.FelhasznaloId == aktualisFelhasznaloId.Value)
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
        public IActionResult ElfogadasVisszavon(int id)
        {
            var valasz = _context.Valaszok.FirstOrDefault(v => v.Id == id);

            if (valasz == null)
                return NotFound();

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

