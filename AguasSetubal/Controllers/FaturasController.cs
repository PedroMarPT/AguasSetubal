using Microsoft.AspNetCore.Mvc;
using AguasSetubal.Data;
using AguasSetubal.Models;
using Microsoft.AspNetCore.Mvc.Rendering; // Para usar SelectList
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            var faturas = _context.Faturas.Include(f => f.Cliente).ToList();
            return View(faturas);
        }

        // GET: Faturas/Create
        public IActionResult Create()
        {
            // Carregar a lista de clientes para o dropdown
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
                try
                {
                    _context.Faturas.Add(fatura);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Tratar exceção de atualização do banco de dados
                    ModelState.AddModelError("", $"Erro ao criar a fatura: {ex.Message}");
                }
            }

            // Repopular o ViewBag.Clientes em caso de erro
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

            // Carregar a lista de clientes para o dropdown
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
                catch (DbUpdateException ex)
                {
                    // Tratar exceção de atualização do banco de dados
                    ModelState.AddModelError("", $"Erro ao atualizar a fatura: {ex.Message}");
                }
                return RedirectToAction(nameof(Index));
            }

            // Repopular o ViewBag.Clientes em caso de erro
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

            if (fatura == null)
            {
                return NotFound();
            }

            try
            {
                _context.Faturas.Remove(fatura);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // Tratar exceção de atualização do banco de dados
                ModelState.AddModelError("", $"Erro ao excluir a fatura: {ex.Message}");
                return View(fatura);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}


