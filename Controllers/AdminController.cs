using Microsoft.AspNetCore.Mvc;
using System.Linq;
using KertKerdes.Data;
using KertKerdes.Models;

namespace KertKerdes.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new AdminViewModel
            {
                VarakozoKerdesek = _context.Kerdesek.Where(x => !x.Jovahagyva).ToList(),
                VarakozoValaszok = _context.Valaszok.Where(x => !x.Jovahagyva).ToList()
            };

            return View(model);
        }

        public IActionResult KerdesJovahagy(int id)
        {
            var k = _context.Kerdesek.Find(id);
            k.Jovahagyva = true;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult KerdesTorles(int id)
        {
            var k = _context.Kerdesek.Find(id);
            _context.Kerdesek.Remove(k);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult ValaszJovahagy(int id)
        {
            var v = _context.Valaszok.Find(id);
            v.Jovahagyva = true;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult ValaszTorles(int id)
        {
            var v = _context.Valaszok.Find(id);
            _context.Valaszok.Remove(v);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}