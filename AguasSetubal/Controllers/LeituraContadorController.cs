using Microsoft.AspNetCore.Mvc;
using AguasSetubal.Models;
using AguasSetubal.Data;
using System.Linq;
using System;

namespace AguasSetubal.Controllers
{
    public class LeituraContadorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeituraContadorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para criar uma nova leitura
        public IActionResult Create(int clienteId)
        {
            var cliente = _context.Clientes.Find(clienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            var leituraAnterior = _context.LeituraContadores
                .Where(l => l.ClienteId == clienteId)
                .OrderByDescending(l => l.DataLeitura)
                .FirstOrDefault();

            var leituraContador = new LeituraContador
            {
                ClienteId = clienteId,
                DataLeituraAnterior = leituraAnterior?.DataLeitura ?? DateTime.Now,
                LeituraAnterior = leituraAnterior?.Valor ?? 0 // Defina a leitura anterior, se disponível
            };

            return View(leituraContador);
        }

        // Método para salvar a nova leitura e calcular o valor a pagar
        [HttpPost]
        public IActionResult Create(LeituraContador leituraContador)
        {
            if (ModelState.IsValid)
            {
                var cliente = _context.Clientes.Find(leituraContador.ClienteId);

                if (cliente == null)
                {
                    return NotFound();
                }

                // Calcular o valor a pagar
                leituraContador.CalcularValorPagar();

                _context.LeituraContadores.Add(leituraContador);
                _context.SaveChanges();

                // Gerar fatura
                var fatura = new Fatura
                {
                    ClienteId = leituraContador.ClienteId,
                    LeituraContadorId = leituraContador.Id,
                    DataEmissao = DateTime.Now,
                    ValorTotal = leituraContador.ValorPagar
                };

                _context.Faturas.Add(fatura);
                _context.SaveChanges();

                return RedirectToAction("Details", "Faturas", new { id = fatura.Id });
            }

            return View(leituraContador);
        }
    }
}


