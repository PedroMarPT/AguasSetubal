using AguasSetubal.Data;
using AguasSetubal.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AguasSetubal.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : Controller
    {
        private readonly IInvoicesRepository _invoicesRepository;
        private readonly IMailHelper _mailHelper;

        public InvoicesController(IInvoicesRepository invoicesRepository, IMailHelper mailHelper)
        {
            _invoicesRepository = invoicesRepository;
            _mailHelper = mailHelper;
        }

        [HttpGet("SendLastInvoice")]
        public IActionResult SendLastInvoice()
        {
            var fatura = _invoicesRepository.GetLastInvoice();
            if (fatura == null)
            {
                return NotFound("Nenhuma fatura em aberto encontrada para este cliente.");
            }

            // Montar a mensagem da fatura
            string mensagem = $@"
            <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2 style='color: #4CAF50;'>Detalhes da Fatura</h2>
                    <p><strong>Fatura ID:</strong> {fatura.Id}</p>
                    <p><strong>Data de Emissão:</strong> {fatura.DataEmissao:dd/MM/yyyy}</p>
                    <p><strong>Valor Total:</strong> <span style='color: #FF5733;'>{fatura.ValorTotal:C}</span></p>
                    <p style='font-size: 12px; color: #FF4500;'>Dirija-se a sua area de cliente para obter dados para pagamento do valor.</p>
                    <hr style='border: 1px solid #ccc;'/>                    
                    <p style='font-size: 12px; color: #888;'>Esta é uma mensagem automática, por favor, não responda.</p>
                </body>
            </html>";

            // Enviar o e-mail
            _mailHelper.SendEmail(fatura.Cliente.Email, "A sua última fatura a pagamento", mensagem);

            return Ok("Fatura enviada com sucesso.");
        }
    }
}