using TaskLogix.Services;

namespace TaskLogix.Factory
{
    public class NotificationFactory : IServiceFactory
    {

        public INotificationService GetNotificationService(Events.Events eventType, GlobalEventArgs globalEventArgs)
        {
            switch (globalEventArgs.EventType)
            {
                case Events.Events.RegisterUser:
                    return new EmailService(globalEventArgs.Data.Email);
                case Events.Events.UpdateUser:
                    Console.WriteLine(globalEventArgs.Data);
                    return default;
                default:
                    throw new ArgumentException("Invalid Event type ");
            }
        }
    }
}