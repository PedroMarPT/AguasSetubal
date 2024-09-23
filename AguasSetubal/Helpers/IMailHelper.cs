namespace AguasSetubal.Helpers
{
    public interface IMailHelper
    {
        void SendEmail(string to, string subject, string body);
    }
}
