using KertKerdes.Data;
using KertKerdes.Models;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public IActionResult Index()
        {
            var model = new AdminViewModel
            {
                VarakozoKerdesek = _context.Kerdesek
                    .Where(k => !k.Jovahagyva)
                    .ToList(),

                VarakozoValaszok = _context.Valaszok
                    .Where(v => !v.Jovahagyva)
                    .ToList()
            };

            return View(model);
        }

        public IActionResult KerdesJovahagy(int id)
        {
            var kerdes = _context.Kerdesek.FirstOrDefault(k => k.Id == id);

            if (kerdes == null)
            {
                return NotFound();
            }

            kerdes.Jovahagyva = true;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult KerdesTorles(int id)
        {
            var kerdes = _context.Kerdesek.FirstOrDefault(k => k.Id == id);

            if (kerdes != null)
            {
                _context.Kerdesek.Remove(kerdes);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult ValaszJovahagy(int id)
        {
            var valasz = _context.Valaszok.FirstOrDefault(v => v.Id == id);

            if (valasz == null)
            {
                return NotFound();
            }

            valasz.Jovahagyva = true;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult ValaszTorles(int id)
        {
            var valasz = _context.Valaszok.FirstOrDefault(v => v.Id == id);

            if (valasz != null)
            {
                _context.Valaszok.Remove(valasz);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}