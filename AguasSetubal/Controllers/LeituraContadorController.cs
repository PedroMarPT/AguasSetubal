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

        // Construtor: Recebe o contexto do banco de dados via injeção de dependência
        public LeituraContadorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método GET: Exibe o formulário para criação de uma nova leitura
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
                LeituraAnterior = leituraAnterior?.Valor ?? 0,
                Cliente = cliente  // Inicializa a propriedade Cliente
            };

            return View(leituraContador);
        }

        // Método POST: Processa o formulário para criação de uma nova leitura
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LeituraContador leituraContador)
        {
            // Verifica se o modelo enviado é válido
            if (ModelState.IsValid)
            {
                // Verifica se o cliente ainda existe no banco de dados
                var cliente = _context.Clientes.Find(leituraContador.ClienteId);
                if (cliente == null)
                {
                    return NotFound();
                }

                // Calcula o valor a pagar com base na leitura atual e anterior
                leituraContador.CalcularValorPagar();

                // Adiciona a nova leitura ao banco de dados
                _context.LeituraContadores.Add(leituraContador);
                _context.SaveChanges();

                // Cria uma nova fatura associada a essa leitura
                var fatura = new Fatura
                {
                    ClienteId = leituraContador.ClienteId,
                   // LeituraContadorId = leituraContador.Id,
                    DataEmissao = DateTime.Now,
                    ValorTotal = leituraContador.ValorPagar,
                    Endereco = cliente.Morada
                };

                // Adiciona a nova fatura ao banco de dados
                _context.Faturas.Add(fatura);
                _context.SaveChanges();

                // Redireciona para a página de detalhes da fatura recém-criada
                return RedirectToAction("Details", "Faturas", new { id = fatura.Id });
            }

            // Se algo deu errado, retorna à view com os dados para correção
            return View(leituraContador);
        }
    }
}






