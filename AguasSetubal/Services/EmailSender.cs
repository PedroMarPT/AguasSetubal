using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace AguasSetubal.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            // Configuração do cliente SMTP
            using (var smtpClient = new SmtpClient(_configuration["Smtp:Host"]))
            {
                smtpClient.Port = int.Parse(_configuration["Smtp:Port"]);
                smtpClient.Credentials = new NetworkCredential(
                    _configuration["Smtp:Username"],
                    _configuration["Smtp:Password"]
                );
                smtpClient.EnableSsl = bool.Parse(_configuration["Smtp:EnableSSL"]);

                // Configuração da mensagem de e-mail
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["Smtp:Username"], _configuration["EmailSettings:SenderName"]),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true, // Permite HTML no corpo do e-mail
                };

                mailMessage.To.Add(email);

                // Envio do e-mail
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}


