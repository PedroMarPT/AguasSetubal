using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AguasSetubal.Data;
using AguasSetubal.Models;
using AguasSetubal.Helpers;
using AguasSetubal.Models.ViewModels;

namespace AguasSetubal.Controllers
{
    public class LeituraContadorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserHelper _userHelper;

        public LeituraContadorController(ApplicationDbContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        // GET: LeituraContadors
        public async Task<IActionResult> Index(string id)
        {
            var listaLeituras = _context.LeituraContadores
                .Include(l => l.Contador)
                .Include(l => l.Cliente);

            if (id != null && listaLeituras != null && listaLeituras.Any())
            {
                var userId = await _userHelper.GetUserByNameAsync(id);

                var clientId = _context.Clientes.FirstOrDefault(c => c.UserId == userId.Id)?.Id;

                var listaLeiturasPorCliente = _context.LeituraContadores
                .Include(f => f.Cliente)
                .Include(f => f.Contador)
                .Where(c => c.ClienteId == clientId);

                return View(await listaLeiturasPorCliente.ToListAsync());
            }

            return View(await listaLeituras.ToListAsync());
        }

        // GET: LeituraContadors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leituraContador = await _context.LeituraContadores
                .Include(l => l.Contador)
                .Include(l => l.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leituraContador == null)
            {
                return NotFound();
            }

            return View(leituraContador);
        }

        // GET: LeituraContadors/Create
        public async Task<IActionResult> CreateAsync(string id)
        {
            LeituraContadorViewModel novaLeitura = new LeituraContadorViewModel();
            novaLeitura.DataLeitura = DateTime.Now;

            var clientes = _context.Clientes;
            ViewBag.Clientes = new SelectList(clientes, "Id", "Nome");

            if (id != null)
            {
                var user = await _userHelper.GetUserByNameAsync(id);

                var clientesPorId = _context.Clientes
                  .Where(c => c.UserId == user.Id && c.UserId != null).ToList();

                ViewBag.Clientes = new SelectList(clientesPorId, "Id", "Nome");
                novaLeitura.UserId = id;
            }

            return View(novaLeitura);
        }

        // POST: LeituraContadors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeituraContadorViewModel leituraContadorVM)
        {
            if (ModelState.IsValid)
            {
                var leituraContador = new LeituraContador
                {
                    LeituraAnterior = leituraContadorVM.LeituraAnterior,
                    LeituraAtual = leituraContadorVM.LeituraAtual,
                    Consumo = leituraContadorVM.Consumo,
                    DataLeitura = leituraContadorVM.DataLeitura,
                    IsInvoiced = leituraContadorVM.IsInvoiced,
                    ClienteId = leituraContadorVM.ClienteId,
                    FaturaId = leituraContadorVM.FaturaId,
                    ContadorId = leituraContadorVM.ContadorId
                };

                _context.Add(leituraContador);
                await _context.SaveChangesAsync();

                if (leituraContadorVM.UserId == string.Empty)
                    return RedirectToAction(nameof(Index));
                else
                    return RedirectToAction(nameof(Index), new { id = leituraContadorVM.UserId });
            }

            ViewData["ContadorId"] = new SelectList(_context.Contador, "Id", "Id", leituraContadorVM.ContadorId);
            ViewData["FaturaId"] = new SelectList(_context.Faturas, "Id", "Id", leituraContadorVM.FaturaId);

            return View(leituraContadorVM);
        }

        // GET: LeituraContadors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leituraContador = await _context.LeituraContadores.FindAsync(id);
            if (leituraContador == null)
            {
                return NotFound();
            }
            ViewData["ContadorId"] = new SelectList(_context.Contador, "Id", "Id", leituraContador.ContadorId);
            ViewData["FaturaId"] = new SelectList(_context.Faturas, "Id", "Id", leituraContador.FaturaId);
            return View(leituraContador);
        }

        // POST: LeituraContadors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeituraContador leituraContador)
        {
            if (id != leituraContador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leituraContador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeituraContadorExists(leituraContador.Id))
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
            ViewData["ContadorId"] = new SelectList(_context.Contador, "Id", "Id", leituraContador.ContadorId);
            ViewData["FaturaId"] = new SelectList(_context.Faturas, "Id", "Id", leituraContador.FaturaId);
            return View(leituraContador);
        }

        // GET: LeituraContadors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leituraContador = await _context.LeituraContadores
                .Include(l => l.Contador)
                .Include(l => l.Fatura)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leituraContador == null)
            {
                return NotFound();
            }

            return View(leituraContador);
        }

        // POST: LeituraContadors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leituraContador = await _context.LeituraContadores.FindAsync(id);
            _context.LeituraContadores.Remove(leituraContador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeituraContadorExists(int id)
        {
            return _context.LeituraContadores.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("LeituraContador/GetCountersByClientId")]
        public JsonResult GetCountersByClientId(int clientId)
        {
            var contadoresCliente = _context.Contador
                .Include(l => l.Cliente)
                .Where(m => m.ClienteId == clientId);

            var list = contadoresCliente.Select(c => new SelectListItem
            {
                Text = c.NumeroContador + " / " + c.NumeroContrato,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Value).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecione contador ...)",
                Value = ""
            });

            return Json(list);
        }

        [HttpPost]
        public JsonResult GetLastReadingByContadorId(int? contadorId)
        {

            var ultimaleitura = _context.LeituraContadores
                .OrderBy(i => i.Id)
                .LastOrDefault(m => m.ContadorId == contadorId);

            var lastReading = ultimaleitura != null ? ultimaleitura.LeituraAtual : 0;

            return Json(new { lastReading = lastReading });
        }

    }
}
