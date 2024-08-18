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
    public class FaturasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FaturasController(ApplicationDbContext context)
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
        public async Task<IActionResult> Create([Bind("ClienteId,LeituraContador.LeituraAnterior,LeituraContador.Valor")] Fatura fatura)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Inicializa LeituraContador caso esteja nulo
                    if (fatura.LeituraContador == null)
                    {
                        fatura.LeituraContador = new LeituraContador();
                    }

                    // Busca o cliente selecionado
                    var cliente = await _context.Clientes.FindAsync(fatura.ClienteId);
                    if (cliente == null)
                    {
                        return NotFound("Cliente não encontrado.");
                    }

                    // Cria uma nova leitura de contador
                    var leitura = new LeituraContador
                    {
                        ClienteId = cliente.Id,
                        DataLeitura = DateTime.Now,
                        LeituraAnterior = fatura.LeituraContador.LeituraAnterior,
                        Valor = fatura.LeituraContador?.Valor ?? 0
                    };

                    // Calcula o consumo (diferença entre a leitura atual e a leitura anterior)
                    leitura.Consumo = leitura.Valor - leitura.LeituraAnterior;
                    leitura.CalcularValorPagar();

                    // Configura a fatura
                    fatura.DataEmissao = DateTime.Now;
                    fatura.Endereco = cliente.Morada;
                    fatura.LeituraContador = leitura;

                    // Calcula o m3Consumo e armazena na fatura
                    fatura.M3Gastos = leitura.Consumo;

                    // Salva as mudanças no banco de dados
                    _context.Add(leitura);
                    _context.Add(fatura);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Lida com erros genéricos
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClienteId,LeituraContador.Valor")] Fatura fatura)
        {
            if (id != fatura.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Busca o cliente selecionado
                    var cliente = await _context.Clientes.FindAsync(fatura.ClienteId);
                    if (cliente == null)
                    {
                        return NotFound("Cliente não encontrado.");
                    }

                    // Busca a leitura do contador relacionada
                    var leitura = await _context.LeituraContadores
                        .FirstOrDefaultAsync(l => l.Id == fatura.LeituraContador.Id);

                    if (leitura == null)
                    {
                        return NotFound("Leitura de contador não encontrada.");
                    }

                    // Atualiza os valores da leitura
                    leitura.Valor = fatura.LeituraContador?.Valor ?? 0;
                    leitura.Consumo = leitura.Valor - leitura.LeituraAnterior;
                    leitura.CalcularValorPagar();

                    // Atualiza as leituras do cliente
                    cliente.LeituraAnteriorContador = cliente.LeituraAtualContador;
                    cliente.LeituraAtualContador = leitura.Valor;

                    // Atualiza a fatura
                    fatura.LeituraContador = leitura;

                    // Atualiza as entradas no banco de dados
                    _context.Update(leitura);
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
                    // Lida com erros genéricos
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

        private bool FaturaExists(int id)
        {
            return _context.Faturas.Any(e => e.Id == id);
        }
    }
}














