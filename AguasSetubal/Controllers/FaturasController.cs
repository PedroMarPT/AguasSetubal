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
            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome", fatura.ClienteId);
            return View(fatura);
        }

        // GET: Faturas/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fatura = _context.Faturas.Find(id);
            if (fatura == null)
            {
                return NotFound();
            }

            // Passa a lista de clientes para a View, para popular o dropdown
            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome", fatura.ClienteId);
            return View(fatura);
        }

        // POST: Faturas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Fatura fatura)
        {
            if (id != fatura.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fatura);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Faturas.Any(f => f.Id == fatura.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Se o ModelState não for válido, precisamos repopular o ViewBag.Clientes
            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome", fatura.ClienteId);
            return View(fatura);
        }

        // GET: Faturas/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fatura = _context.Faturas
                .Include(f => f.Cliente)
                .FirstOrDefault(m => m.Id == id);
            if (fatura == null)
            {
                return NotFound();
            }

            return View(fatura);
        }

        // GET: Faturas/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fatura = _context.Faturas
                .Include(f => f.Cliente)
                .FirstOrDefault(m => m.Id == id);
            if (fatura == null)
            {
                return NotFound();
            }

            return View(fatura);
        }

        // POST: Faturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var fatura = _context.Faturas.Find(id);
            _context.Faturas.Remove(fatura);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}



