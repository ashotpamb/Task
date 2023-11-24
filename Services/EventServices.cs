


namespace TaskLogix.Services
{
    public class EventService : IEventService
    {
        private readonly ILogger<EventService> _logger;

        public event EventHandler<GlobalEventArgs> GlobalEventHandler;

        public EventService(ILogger<EventService> logger)
        {
            _logger = logger;
        }


        public void InvokeEvent(Events.Events eventType, dynamic data)
        {
            if (GlobalEventHandler != null)
            {
                GlobalEventHandler.Invoke(this, new GlobalEventArgs(eventType, data));
            }
            else
            {
                _logger.LogInformation($"Event has no subscribers.");
            }
        }

    }

}