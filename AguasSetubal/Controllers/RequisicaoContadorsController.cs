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
    public class RequisicaoContadorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequisicaoContadorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RequisicaoContador
        public async Task<IActionResult> Index()
        {
            return View(await _context.RequisicaoContador.ToListAsync());
        }

        // GET: RequisicaoContador/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisicaoContador = await _context.RequisicaoContador
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requisicaoContador == null)
            {
                return NotFound();
            }

            return View(requisicaoContador);
        }

        // GET: RequisicaoContador/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RequisicaoContador/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RequisicaoContador requisicaoContador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requisicaoContador);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home", new { message = "Requisição de contador registado com sucesso. Será contatato em breve pelos nossos serviços. Obrigado pela confiamça!" });
            }
            return View(requisicaoContador);
        }

        // GET: RequisicaoContador/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisicaoContador = await _context.RequisicaoContador.FindAsync(id);
            if (requisicaoContador == null)
            {
                return NotFound();
            }
            return View(requisicaoContador);
        }

        // POST: RequisicaoContador/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RequisicaoContador requisicaoContador)
        {
            if (id != requisicaoContador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requisicaoContador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequisicaoContadorExists(requisicaoContador.Id))
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
            return View(requisicaoContador);
        }

        // GET: RequisicaoContador/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisicaoContador = await _context.RequisicaoContador
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requisicaoContador == null)
            {
                return NotFound();
            }

            return View(requisicaoContador);
        }

        // POST: RequisicaoContador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requisicaoContador = await _context.RequisicaoContador.FindAsync(id);
            _context.RequisicaoContador.Remove(requisicaoContador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequisicaoContadorExists(int id)
        {
            return _context.RequisicaoContador.Any(e => e.Id == id);
        }
    }
}
