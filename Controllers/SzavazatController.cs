using KertKerdes.Data;
using KertKerdes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace KertKerdes.Controllers
{
    public class SzavazatController : Controller
    {
        private readonly AppDbContext _context;

        public SzavazatController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult KerdesFel(int id)
        {
            var felhasznaloId = HttpContext.Session.GetInt32("FelhasznaloId");

            if (felhasznaloId == null)
            {
                TempData["BelepesSzukseges"] = true;
                return RedirectToAction("Bejelentkezes", "Fiok");
            }

            var kerdes = _context.Kerdesek.FirstOrDefault(k => k.Id == id);

            if (kerdes == null)
                return NotFound();

            if (kerdes.FelhasznaloId == felhasznaloId.Value)
                return RedirectToAction("Index", "Kerdes");

            var marSzavazott = _context.Szavazatok.FirstOrDefault(s =>
                s.FelhasznaloId == felhasznaloId.Value &&
                s.KerdesId == id);

            if (marSzavazott != null)
                return RedirectToAction("Index", "Kerdes");

            kerdes.Szavazat++;

            _context.Szavazatok.Add(new Szavazat
            {
                FelhasznaloId = felhasznaloId.Value,
                KerdesId = id,
                Ertek = 1
            });

            _context.SaveChanges();

            return RedirectToAction("Index", "Kerdes");
        }

        public IActionResult KerdesLe(int id)
        {
            var felhasznaloId = HttpContext.Session.GetInt32("FelhasznaloId");

            if (felhasznaloId == null)
            {
                TempData["BelepesSzukseges"] = true;
                return RedirectToAction("Bejelentkezes", "Fiok");
            }

            var kerdes = _context.Kerdesek.FirstOrDefault(k => k.Id == id);

            if (kerdes == null)
                return NotFound();

            if (kerdes.FelhasznaloId == felhasznaloId.Value)
                return RedirectToAction("Index", "Kerdes");

            var marSzavazott = _context.Szavazatok.FirstOrDefault(s =>
                s.FelhasznaloId == felhasznaloId.Value &&
                s.KerdesId == id);

            if (marSzavazott != null)
                return RedirectToAction("Index", "Kerdes");

            kerdes.Szavazat--;

            _context.Szavazatok.Add(new Szavazat
            {
                FelhasznaloId = felhasznaloId.Value,
                KerdesId = id,
                Ertek = -1
            });

            _context.SaveChanges();

            return RedirectToAction("Index", "Kerdes");
        }

        public IActionResult ValaszFel(int id)
        {
            var felhasznaloId = HttpContext.Session.GetInt32("FelhasznaloId");

            if (felhasznaloId == null)
            {
                TempData["BelepesSzukseges"] = true;
                return RedirectToAction("Bejelentkezes", "Fiok");
            }
            var valasz = _context.Valaszok.FirstOrDefault(v => v.Id == id);

            if (valasz == null)
                return NotFound();

            if (valasz.FelhasznaloId == felhasznaloId.Value)
                return RedirectToAction("Reszletek", "Kerdes", new { id = valasz.KerdesId });

            var marSzavazott = _context.Szavazatok.FirstOrDefault(s =>
                s.FelhasznaloId == felhasznaloId.Value &&
                s.ValaszId == id);

            if (marSzavazott != null)
                return RedirectToAction("Reszletek", "Kerdes", new { id = valasz.KerdesId });

            valasz.Szavazat++;

            _context.Szavazatok.Add(new Szavazat
            {
                FelhasznaloId = felhasznaloId.Value,
                ValaszId = id,
                Ertek = 1
            });

            _context.SaveChanges();

            return RedirectToAction("Reszletek", "Kerdes", new { id = valasz.KerdesId });
        }

        public IActionResult ValaszLe(int id)
        {
            var felhasznaloId = HttpContext.Session.GetInt32("FelhasznaloId");

            if (felhasznaloId == null)
            {
                TempData["BelepesSzukseges"] = true;
                return RedirectToAction("Bejelentkezes", "Fiok");
            }

            var valasz = _context.Valaszok.FirstOrDefault(v => v.Id == id);

            if (valasz == null)
                return NotFound();

            if (valasz.FelhasznaloId == felhasznaloId.Value)
                return RedirectToAction("Reszletek", "Kerdes", new { id = valasz.KerdesId });

            var marSzavazott = _context.Szavazatok.FirstOrDefault(s =>
                s.FelhasznaloId == felhasznaloId.Value &&
                s.ValaszId == id);

            if (marSzavazott != null)
                return RedirectToAction("Reszletek", "Kerdes", new { id = valasz.KerdesId });

            valasz.Szavazat--;

            _context.Szavazatok.Add(new Szavazat
            {
                FelhasznaloId = felhasznaloId.Value,
                ValaszId = id,
                Ertek = -1
            });

            _context.SaveChanges();

            return RedirectToAction("Reszletek", "Kerdes", new { id = valasz.KerdesId });
        }
    }
}