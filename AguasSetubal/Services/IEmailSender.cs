using System.Threading.Tasks;

namespace AguasSetubal.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
