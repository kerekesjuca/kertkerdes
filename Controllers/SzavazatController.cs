using KertKerdes.Data;
using KertKerdes.Models;
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
            {
                TempData["SzavazatHiba"] = "Saját kérdésre nem szavazhatsz.";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            var marSzavazott = _context.Szavazatok.FirstOrDefault(s =>
                s.FelhasznaloId == felhasznaloId.Value &&
                s.KerdesId == id);

            if (marSzavazott != null)
            {
                TempData["SzavazatHiba"] = "Erre a kérdésre már szavaztál.";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            kerdes.Szavazat++;

            _context.Szavazatok.Add(new Szavazat
            {
                FelhasznaloId = felhasznaloId.Value,
                KerdesId = id,
                Ertek = 1
            });

            _context.SaveChanges();

            TempData["SzavazatSiker"] = "Szavazat rögzítve.";

            return Redirect(Request.Headers["Referer"].ToString());
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
            {
                TempData["SzavazatHiba"] = "Saját kérdésre nem szavazhatsz.";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            var marSzavazott = _context.Szavazatok.FirstOrDefault(s =>
                s.FelhasznaloId == felhasznaloId.Value &&
                s.KerdesId == id);

            if (marSzavazott != null)
            {
                TempData["SzavazatHiba"] = "Erre a kérdésre már szavaztál.";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            kerdes.Szavazat--;

            _context.Szavazatok.Add(new Szavazat
            {
                FelhasznaloId = felhasznaloId.Value,
                KerdesId = id,
                Ertek = -1
            });

            _context.SaveChanges();

            TempData["SzavazatSiker"] = "Szavazat rögzítve.";

            return Redirect(Request.Headers["Referer"].ToString());
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

            if (!valasz.Jovahagyva)
                return Redirect(Request.Headers["Referer"].ToString());

            if (valasz.FelhasznaloId == felhasznaloId.Value)
            {
                TempData["SzavazatHiba"] = "Saját válaszra nem szavazhatsz.";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            var marSzavazott = _context.Szavazatok.FirstOrDefault(s =>
                s.FelhasznaloId == felhasznaloId.Value &&
                s.ValaszId == id);

            if (marSzavazott != null)
            {
                TempData["SzavazatHiba"] = "Erre a válaszra már szavaztál.";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            valasz.Szavazat++;

            _context.Szavazatok.Add(new Szavazat
            {
                FelhasznaloId = felhasznaloId.Value,
                ValaszId = id,
                Ertek = 1
            });

            _context.SaveChanges();

            TempData["SzavazatSiker"] = "Szavazat rögzítve.";

            return Redirect(Request.Headers["Referer"].ToString());
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

            if (!valasz.Jovahagyva)
                return Redirect(Request.Headers["Referer"].ToString());

            if (valasz.FelhasznaloId == felhasznaloId.Value)
            {
                TempData["SzavazatHiba"] = "Saját válaszra nem szavazhatsz.";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            var marSzavazott = _context.Szavazatok.FirstOrDefault(s =>
                s.FelhasznaloId == felhasznaloId.Value &&
                s.ValaszId == id);

            if (marSzavazott != null)
            {
                TempData["SzavazatHiba"] = "Erre a válaszra már szavaztál.";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            valasz.Szavazat--;

            _context.Szavazatok.Add(new Szavazat
            {
                FelhasznaloId = felhasznaloId.Value,
                ValaszId = id,
                Ertek = -1
            });

            _context.SaveChanges();

            TempData["SzavazatSiker"] = "Szavazat rögzítve.";

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}