
namespace TaskLogix.Services
{
    public interface IEventService
    {
        public event EventHandler<GlobalEventArgs> GlobalEventHandler;
        public void InvokeEvent(Events.Events eventType, dynamic data);

    }
}