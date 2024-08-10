using Microsoft.AspNetCore.Mvc;
using AguasSetubal.Data;
using AguasSetubal.Models;
using Microsoft.AspNetCore.Mvc.Rendering; // Para usar SelectList
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AguasSetubal.Controllers
{
    public class FaturasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FaturasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Faturas
        public IActionResult Index()
        {
            // Inclua o cliente na consulta para exibir informações sobre o cliente na lista de faturas
            var faturas = _context.Faturas.Include(f => f.Cliente).ToList();
            return View(faturas);
        }

        // GET: Faturas/Create
        public IActionResult Create()
        {
            // Passa a lista de clientes para a View, para popular o dropdown
            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome");
            return View();
        }

        // POST: Faturas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Fatura fatura)
        {
            if (ModelState.IsValid)
            {
                _context.Faturas.Add(fatura);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // Se o ModelState não for válido, precisamos repopular o ViewBag.Clientes
            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome");
            return View(fatura);
        }
    }
}



