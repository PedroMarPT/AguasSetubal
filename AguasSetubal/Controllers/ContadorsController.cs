using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AguasSetubal.Data;
using AguasSetubal.Models;

namespace AguasSetubal.Controllers
{
    public class ContadorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContadorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contadors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Contador.Include(c => c.Cliente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Contadors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contador = await _context.Contador
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contador == null)
            {
                return NotFound();
            }

            return View(contador);
        }

        // GET: Contadors/Create
        public IActionResult Create()
        {
            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome");
            return View();
        }

        // POST: Contadors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contador contador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", contador.ClienteId);
            return View(contador);
        }

        // GET: Contadors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contador = await _context.Contador.FindAsync(id);
            if (contador == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", contador.ClienteId);
            return View(contador);
        }

        // POST: Contadors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contador contador)
        {
            if (id != contador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContadorExists(contador.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", contador.ClienteId);
            return View(contador);
        }

        // GET: Contadors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contador = await _context.Contador
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contador == null)
            {
                return NotFound();
            }

            return View(contador);
        }

        // POST: Contadors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contador = await _context.Contador.FindAsync(id);
            _context.Contador.Remove(contador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContadorExists(int id)
        {
            return _context.Contador.Any(e => e.Id == id);
        }
    }
}