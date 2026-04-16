using KertKerdes.Data;
using KertKerdes.Models;
using KertKerdes.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KertKerdes.Controllers
{
    public class FiokController : Controller
    {
        private readonly AppDbContext _context;

        public FiokController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Regisztracio()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Regisztracio(RegisztracioViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var email = model.Email.Trim().ToLower();
            var felhasznalonev = model.Felhasznalonev.Trim();

            if (CimkeModeraloHelper.TiltottCimke(felhasznalonev))
            {
                ModelState.AddModelError("Felhasznalonev", "A felhasználónév trágár kifejezést tartalmaz, módosítsd.");
                return View(model);
            }

            var letezoEmail = _context.Felhasznalok
                .FirstOrDefault(f => f.Email.ToLower() == email);

            if (letezoEmail != null)
            {
                ModelState.AddModelError("Email", "Ezzel az email címmel már létezik regisztráció.");
                return View(model);
            }

            var letezoFelhasznalonev = _context.Felhasznalok
                .FirstOrDefault(f => f.Felhasznalonev.ToLower() == felhasznalonev.ToLower());

            if (letezoFelhasznalonev != null)
            {
                ModelState.AddModelError("Felhasznalonev", "Ez a felhasználónév már foglalt.");
                return View(model);
            }

            var ervenyesSzerepkor = model.Szerepkor == "Altalanos" || model.Szerepkor == "Moderator"
                ? model.Szerepkor
                : "Altalanos";

            var ujFelhasznalo = new Felhasznalo
            {
                Felhasznalonev = felhasznalonev,
                Email = email,
                JelszoHash = JelszoHelper.HashJelszo(model.Jelszo.Trim()),
                Szerepkor = ervenyesSzerepkor,
                ModeratorJovahagyva = ervenyesSzerepkor == "Altalanos",
                ModeratorElutasitva = false,
                RegisztracioDatuma = DateTime.Now
            };

            _context.Felhasznalok.Add(ujFelhasznalo);
            _context.SaveChanges();

            TempData["RegisztracioTipus"] = ervenyesSzerepkor;

            return RedirectToAction("Regisztracio");
        }

        public IActionResult Bejelentkezes()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Bejelentkezes(BejelentkezesViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Jelszo))
            {
                ViewBag.Hiba = "Hibás belépési adatok.";
                return View(model);
            }

            var email = model.Email.Trim().ToLower();
            var jelszo = model.Jelszo.Trim();

            var felhasznalo = _context.Felhasznalok
                .FirstOrDefault(f => f.Email.ToLower() == email);

            if (felhasznalo == null)
            {
                ViewBag.Hiba = "Hibás belépési adatok.";
                return View(model);
            }

            var hash = JelszoHelper.HashJelszo(jelszo);

            if (felhasznalo.JelszoHash.ToLower() != hash.ToLower())
            {
                ViewBag.Hiba = "Hibás belépési adatok.";
                return View(model);
            }

            if (felhasznalo.ModeratorElutasitva)
            {
                ViewBag.Hiba = "A moderátori kérelmet elutasította az admin.";
                return View(model);
            }

            if (felhasznalo.Szerepkor == "Moderator" && !felhasznalo.ModeratorJovahagyva)
            {
                ViewBag.Hiba = "A moderátori jogosultság még nincs jóváhagyva.";
                return View(model);
            }

            HttpContext.Session.SetString("FelhasznaloNev", felhasznalo.Felhasznalonev);
            HttpContext.Session.SetString("Szerepkor", felhasznalo.Szerepkor);
            HttpContext.Session.SetInt32("FelhasznaloId", felhasznalo.Id);

            return RedirectToAction("Index", "Kerdes");
        }

        public IActionResult Kijelentkezes()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Kerdes");
        }
    }
}