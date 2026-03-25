using Microsoft.AspNetCore.Mvc;
using KertKerdes.Data;
using KertKerdes.Models;
using System;

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
            valasz.Jovahagyva = false;
            valasz.Elfogadott = false;
            valasz.Szavazat = 0;
            valasz.FelhasznaloId = 1;
            valasz.Datum = DateTime.Now;

            _context.Valaszok.Add(valasz);
            _context.SaveChanges();

            return RedirectToAction("Reszletek", "Kerdes", new { id = valasz.KerdesId });
        }

        public IActionResult Elfogad(int id)
        {
            var valasz = _context.Valaszok.FirstOrDefault(v => v.Id == id);

            if (valasz == null)
            {
                return NotFound();
            }

            valasz.Elfogadott = true;

            _context.SaveChanges();

            return RedirectToAction("Reszletek", "Kerdes", new { id = valasz.KerdesId });
        }
    }
}