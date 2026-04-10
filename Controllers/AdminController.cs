using KertKerdes.Data;
using KertKerdes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KertKerdes.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        private bool ModeratorVagyAdmin()
        {
            var szerepkor = HttpContext.Session.GetString("Szerepkor");
            return szerepkor == "Admin" || szerepkor == "Moderator";
        }

        private bool CsakAdmin()
        {
            var szerepkor = HttpContext.Session.GetString("Szerepkor");
            return szerepkor == "Admin";
        }

        public IActionResult Index()
        {
            if (!ModeratorVagyAdmin())
                return RedirectToAction("Index", "Kerdes");

            var szerepkor = HttpContext.Session.GetString("Szerepkor");

            var model = new AdminViewModel
            {
                VarakozoKerdesek = _context.Kerdesek
                    .Include(k => k.Felhasznalo)
                    .Where(k => !k.Jovahagyva)
                    .ToList(),

                VarakozoValaszok = _context.Valaszok
                    .Include(v => v.Felhasznalo)
                    .Include(v => v.Kerdes)
                    .Where(v => !v.Jovahagyva)
                    .ToList(),

                VarakozoModeratorok = szerepkor == "Admin"
                    ? _context.Felhasznalok
                        .Where(f => f.Szerepkor == "Moderator" && !f.ModeratorJovahagyva && !f.ModeratorElutasitva)
                        .ToList()
                    : new List<Felhasznalo>()
            };

            return View(model);
        }

        public IActionResult KerdesJovahagy(int id)
        {
            if (!ModeratorVagyAdmin())
                return RedirectToAction("Index", "Kerdes");

            var kerdes = _context.Kerdesek.FirstOrDefault(k => k.Id == id);

            if (kerdes == null)
                return NotFound();

            kerdes.Jovahagyva = true;

            _context.SaveChanges();

            TempData["AdminSiker"] = "A kérdés jóváhagyva.";

            return RedirectToAction("Index");
        }

        public IActionResult KerdesTorles(int id)
        {
            if (!ModeratorVagyAdmin())
                return RedirectToAction("Index", "Kerdes");

            var kerdes = _context.Kerdesek.FirstOrDefault(k => k.Id == id);

            if (kerdes != null)
            {
                _context.Kerdesek.Remove(kerdes);
                _context.SaveChanges();
            }

            TempData["AdminSiker"] = "A kérdés törölve.";

            return RedirectToAction("Index");
        }

        public IActionResult ValaszJovahagy(int id)
        {
            if (!ModeratorVagyAdmin())
                return RedirectToAction("Index", "Kerdes");

            var valasz = _context.Valaszok.FirstOrDefault(v => v.Id == id);

            if (valasz == null)
                return NotFound();

            valasz.Jovahagyva = true;

            _context.SaveChanges();

            TempData["AdminSiker"] = "A válasz jóváhagyva.";

            return RedirectToAction("Index");
        }

        public IActionResult ValaszTorles(int id)
        {
            if (!ModeratorVagyAdmin())
                return RedirectToAction("Index", "Kerdes");

            var valasz = _context.Valaszok.FirstOrDefault(v => v.Id == id);

            if (valasz != null)
            {
                _context.Valaszok.Remove(valasz);
                _context.SaveChanges();
            }

            TempData["AdminSiker"] = "A válasz törölve.";

            return RedirectToAction("Index");
        }

        public IActionResult ModeratorJovahagy(int id)
        {
            if (!CsakAdmin())
            {
                TempData["AdminHiba"] = "Moderátort csak admin hagyhat jóvá.";
                return RedirectToAction("Index");
            }

            var felhasznalo = _context.Felhasznalok.FirstOrDefault(f => f.Id == id);

            if (felhasznalo == null)
                return NotFound();

            felhasznalo.ModeratorJovahagyva = true;
            felhasznalo.ModeratorElutasitva = false;

            _context.SaveChanges();

            TempData["AdminSiker"] = "Moderátor kérelem jóváhagyva.";

            return RedirectToAction("Index");
        }

        public IActionResult ModeratorElutasit(int id)
        {
            if (!CsakAdmin())
            {
                TempData["AdminHiba"] = "Moderátor kérelmet csak admin utasíthat el.";
                return RedirectToAction("Index");
            }

            var felhasznalo = _context.Felhasznalok.FirstOrDefault(f => f.Id == id);

            if (felhasznalo == null)
                return NotFound();

            felhasznalo.ModeratorElutasitva = true;
            felhasznalo.ModeratorJovahagyva = false;

            _context.SaveChanges();

            TempData["AdminSiker"] = "Moderátor kérelem elutasítva.";

            return RedirectToAction("Index");
        }
    }
}