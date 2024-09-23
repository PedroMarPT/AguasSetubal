using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Net;
using System;
using System.Net.Mail;
using MailKit.Net.Smtp;

namespace AguasSetubal.Helpers
{
    public class MailHelper : IMailHelper
    {

        private readonly IConfiguration _configuration;

        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string to, string subject, string body)
        {
            var nameFrom = _configuration["Mail: NameFrom"];
            var from = _configuration["Mail: From"];
            var smtp = _configuration["Mail: Smtp"];
            var port = _configuration["Mail: Port"];
            var password = _configuration["Mail: Password"];

            var message = new MailMessage();
            message.From = new MailAddress(nameFrom, from);
            message.To.Add(new MailAddress(to, to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };

            message.Body = body ?? "";
            message.IsBodyHtml = true;

            using (var client = new System.Net.Mail.SmtpClient(smtp, int.Parse(port)))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(from, password);
                client.EnableSsl = false;
                client.Send(message);
            }

        }
    }
}