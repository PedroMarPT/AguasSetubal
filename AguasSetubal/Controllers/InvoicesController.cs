using Microsoft.AspNetCore.Mvc;
using AguasSetubal.Data;
using AguasSetubal.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace AguasSetubal.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Faturas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Faturas
                .Include(f => f.Cliente)
                .Include(f => f.LeituraContador);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Faturas/Create
        public IActionResult Create()
        {
            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome");
            return View(new Fatura
            {
                LeituraContador = new LeituraContador()  // Inicializa o objeto para evitar null references
            });
        }

        // POST: Faturas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Fatura fatura)
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

                    var leituraAnterior = fatura.LeituraAnterior;
                    var leituraAtual = fatura.LeituraAtual;

                    // Cria uma nova leitura de contador
                    var leitura = new LeituraContador
                    {
                        ClienteId = cliente.Id,
                        DataLeitura = DateTime.Now,
                        LeituraAnterior = leituraAnterior,
                        Valor = leituraAtual
                    };

                    leitura.Consumo = leituraAtual - leituraAnterior;

                    fatura.DataEmissao = DateTime.Now;
                    fatura.Endereco = cliente.Morada;
                    fatura.LeituraAnterior = leituraAnterior;
                    fatura.LeituraAtual = leituraAtual;
                    fatura.LeituraContador = leitura;

                    _context.Add(leitura);
                    _context.Add(fatura);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao criar a fatura: " + ex.Message);
                }
            }

            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome", fatura.ClienteId);
            return View(fatura);
        }



        // GET: Faturas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fatura = await _context.Faturas
                .Include(f => f.LeituraContador)
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

                    _context.Update(fatura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaturaExists(fatura.Id))
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
                .Include(f => f.LeituraContador)
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
                .Include(f => f.LeituraContador)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (fatura == null)
            {
                return NotFound();
            }

            return View(fatura);
        }

        // POST: Faturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fatura = await _context.Faturas.FindAsync(id);
            if (fatura != null)
            {
                _context.Faturas.Remove(fatura);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ImprimirFatura(int id)
        {
            var fatura = await _context.Faturas
                .Include(f => f.Cliente)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fatura == null)
            {
                return NotFound();
            }

            // Passa a fatura para a view
            return View(fatura);
        }


        private bool FaturaExists(int id)
        {
            return _context.Faturas.Any(e => e.Id == id);
        }
    }
}















