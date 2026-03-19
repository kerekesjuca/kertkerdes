using Microsoft.AspNetCore.Mvc;
using KertKerdes.Data;
using KertKerdes.Models;

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
        public IActionResult Letrehozas(Valasz v)
        {
            v.Szerzo = "felhasznalo";
            v.Jovahagyva = false;

            _context.Valaszok.Add(v);
            _context.SaveChanges();

            return RedirectToAction("Reszletek", "Kerdes", new { id = v.KerdesId });
        }

        public IActionResult Elfogad(int id)
        {
            var v = _context.Valaszok.Find(id);
            v.Elfogadott = true;
            _context.SaveChanges();

            return RedirectToAction("Reszletek", "Kerdes", new { id = v.KerdesId });
        }
    }
}