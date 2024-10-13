using Microsoft.AspNetCore.Mvc;
using AguasSetubal.Data;
using AguasSetubal.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using AguasSetubal.Helpers;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using static Fable.Core.JS;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace AguasSetubal.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly IInvoicesRepository _invoicesRepository;
        private readonly ApplicationDbContext _context;
        private readonly IUserHelper _userHelper;

        public InvoicesController(ApplicationDbContext context, IInvoicesRepository invoicesRepository, IUserHelper userHelper)
        {
            _context = context;
            _invoicesRepository = invoicesRepository;
            _userHelper = userHelper;
        }


        // GET: Faturas
        public async Task<IActionResult> Index(string id)
        {
            var listaFaturas = _context.Faturas
                .Include(f => f.Cliente)
                .Include(f => f.LeiturasContador);

            if (id != null && listaFaturas != null && listaFaturas.Any())
            {
                var userId = await _userHelper.GetUserByNameAsync(id);

                var clientId = _context.Clientes.FirstOrDefault(c => c.UserId == userId.Id)?.Id;

                if (clientId != null)
                {
                    var listaFaturasPorCliente = _context.Faturas
                    .Include(f => f.Cliente)
                    .Include(f => f.LeiturasContador)
                    .Where(c => c.ClienteId == clientId);

                    return View(await listaFaturasPorCliente.ToListAsync());
                }
                else
                {
                    var lista = new List<Fatura>();
                    return View(lista);
                }
            }

            return View(await listaFaturas.ToListAsync());
        }

        [Authorize]
        [Authorize(Roles = "Funcionario")]
        // GET: Faturas/Create
        public IActionResult Create()
        {
            Fatura novaFatura = new Fatura();
            novaFatura.DataInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            novaFatura.DataFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome");

            return View(novaFatura);
        }

        // POST: Faturas/Create
        [Authorize]
        [Authorize(Roles = "Funcionario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Fatura fatura)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var cliente = await _context.Clientes.FindAsync(fatura.ClienteId);
                    if (cliente == null)
                    {
                        return NotFound("Cliente não encontrado.");
                    }

                    fatura.Cliente = cliente;

                    var contador = await _context.Contador.FindAsync(fatura.ContadorId);
                    if (contador == null)
                    {
                        return NotFound("Contador não encontrado.");
                    }

                    var tabelaPrecos = _context.TabelaPrecos.ToList();
                    if (!tabelaPrecos.Any())
                    {
                        return NotFound("Tabela preços vazia.");
                    }

                    fatura.Contador = contador;

                    var listaLeituras = _context.LeituraContadores
                        .Where(c => c.ContadorId == fatura.ContadorId
                        && c.IsInvoiced == false
                        && c.DataLeitura.Year >= fatura.DataInicio.Year
                        && c.DataLeitura.Month >= fatura.DataInicio.Month
                        && c.DataLeitura.Day >= fatura.DataInicio.Day
                        && c.DataLeitura.Year <= fatura.DataFim.Year
                        && c.DataLeitura.Month <= fatura.DataFim.Month
                        && c.DataLeitura.Day <= fatura.DataFim.Day).ToList();

                    foreach (var leitura in listaLeituras)
                    {
                        fatura.ConsumoTotal += (leitura.LeituraAtual - leitura.LeituraAnterior);
                        fatura.LeiturasContador.Add(leitura);
                    }

                    foreach (var item in tabelaPrecos)
                    {
                        if (fatura.ConsumoTotal > item.LimiteInferior
                            && (fatura.ConsumoTotal <= item.LimiteSuperior || item.LimiteSuperior == 0))
                        {
                            fatura.Descritivo = item.NomeEscalao;
                            fatura.ValorTotal = item.ValorUnitario * fatura.ConsumoTotal;
                            fatura.ValorUnitario = item.ValorUnitario;
                        }
                    }

                    fatura.DataEmissao = DateTime.Now;

                    await _invoicesRepository.CreateAsync(fatura);

                    foreach (var leitura in listaLeituras)
                    {
                        leitura.IsInvoiced = true;
                        _context.Update(leitura);
                    }
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao criar a fatura: " + ex.Message);
                }
            }

            ViewBag.Clientes = new SelectList(_context.Clientes.Distinct(), "Id", "Nome", fatura.ClienteId);
            return View(fatura);
        }



        // GET: Faturas/Edit/5
        [Authorize]
        [Authorize(Roles = "Funcionario")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fatura = await _context.Faturas
                .Include(f => f.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (fatura == null)
            {
                return NotFound();
            }

            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome", fatura.ClienteId);
            return View(fatura);
        }


        // POST: Faturas/Edit/5
        [Authorize]
        [Authorize(Roles = "Funcionario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Fatura fatura)
        {
            if (id != fatura.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cliente = await _context.Clientes.FindAsync(fatura.ClienteId);
                    if (cliente == null)
                    {
                        return NotFound("Cliente não encontrado.");
                    }
                    fatura.Cliente = cliente;

                    await _invoicesRepository.UpdateAsync(fatura);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _invoicesRepository.ExistsAsync(fatura.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao editar a fatura: " + ex.Message);
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome", fatura.ClienteId);
            return View(fatura);
        }

        // GET: Faturas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fatura = await _context.Faturas
                .Include(f => f.Cliente)
                .Include(f => f.Contador)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (fatura == null)
            {
                return NotFound();
            }

            return View(fatura);
        }

        // GET: Faturas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fatura = await _context.Faturas
                .Include(f => f.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (fatura == null)
            {
                return NotFound();
            }

            return View(fatura);
        }

        // POST: Faturas/Delete/5
        [Authorize]
        [Authorize(Roles = "Funcionario")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fatura = await _invoicesRepository.GetByIdAsync(id);
            if (fatura != null)
            {
                await _invoicesRepository.DeleteAsync(fatura);
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> ImprimirFatura(int id)
        {
            var fatura = await _context.Faturas
                .Include(f => f.Cliente)
                .Include(f => f.Contador)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fatura == null)
            {
                return NotFound();
            }

            // Passa a fatura para a view
            return View(fatura);
        }


        [HttpGet]
        public JsonResult GetLastReadingByClientId(int? id)
        {
            var ultimaleitura = _context.LeituraContadores
                .OrderBy(i => i.Id)
                .LastOrDefault(m => m.ContadorId == id);

            var lastReading = ultimaleitura != null ? ultimaleitura.LeituraAtual : 0;

            return Json(new { lastReading = lastReading });
        }
    }
}















