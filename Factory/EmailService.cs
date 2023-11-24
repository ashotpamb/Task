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
            await Task.Delay(3000);
            Console.WriteLine($"Sending email notification: {To}");

        }
    }
}