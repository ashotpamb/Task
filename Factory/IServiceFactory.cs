using TaskLogix.Services;

namespace TaskLogix.Factory
{
    public interface IServiceFactory
    {
        INotificationService GetNotificationService(Events.Events eventType, GlobalEventArgs globalEventArgs);
    }
}