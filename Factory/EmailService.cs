using System.Net.Mail;
using TaskLogix.Factory;

namespace TaskLogix.Services
{
    public class EmailService : INotificationService
    {
        public string To {get;set;}

        public EmailService(string to)
        {
            To = to;
        }

        public async Task SendNotificationAsync()
        {
            // using (SmtpClient smtpClient = new SmtpClient())
            // {
            //     
            // }
            await Task.Delay(1000);
            Console.WriteLine($"Sending email notification: {To}");

        }
    }
}