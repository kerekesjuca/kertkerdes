using Microsoft.AspNetCore.Mvc;
using KertKerdes.Data;
using System.Linq;

namespace KertKerdes.Controllers
{
    public class StatisztikaController : Controller
    {
        private readonly AppDbContext _context;

        public StatisztikaController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.KerdesDb = _context.Kerdesek.Count(k => k.Jovahagyva);
            ViewBag.ValaszDb = _context.Valaszok.Count(v => v.Jovahagyva);

            return View();
        }
    }
}