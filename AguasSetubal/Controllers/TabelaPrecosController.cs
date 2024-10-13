using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AguasSetubal.Data;
using AguasSetubal.Models;
using Microsoft.AspNetCore.Authorization;


namespace AguasSetubal.Controllers
{
    public class TabelaPrecosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TabelaPrecosController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: TabelaPrecos
        [Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.TabelaPrecos.ToListAsync());
        }

        // GET: TabelaPrecos/Details/5
        [Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tabelaPrecos = await _context.TabelaPrecos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tabelaPrecos == null)
            {
                return NotFound();
            }

            return View(tabelaPrecos);
        }

        // GET: TabelaPrecos/Create
        [Authorize]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TabelaPrecos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TabelaPrecos tabelaPrecos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tabelaPrecos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tabelaPrecos);
        }

        // GET: TabelaPrecos/Edit/5
        [Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tabelaPrecos = await _context.TabelaPrecos.FindAsync(id);
            if (tabelaPrecos == null)
            {
                return NotFound();
            }
            return View(tabelaPrecos);
        }

        // POST: TabelaPrecos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TabelaPrecos tabelaPrecos)
        {
            if (id != tabelaPrecos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tabelaPrecos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TabelaPrecosExists(tabelaPrecos.Id))
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
            return View(tabelaPrecos);
        }

        // GET: TabelaPrecos/Delete/5
        [Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tabelaPrecos = await _context.TabelaPrecos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tabelaPrecos == null)
            {
                return NotFound();
            }

            return View(tabelaPrecos);
        }

        // POST: TabelaPrecos/Delete/5
        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tabelaPrecos = await _context.TabelaPrecos.FindAsync(id);
            _context.TabelaPrecos.Remove(tabelaPrecos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TabelaPrecosExists(int id)
        {
            return _context.TabelaPrecos.Any(e => e.Id == id);
        }
    }
}
