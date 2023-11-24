namespace TaskLogix.Services
{
    public class GlobalEventArgs : EventArgs
    {
        public Events.Events EventType { get; }
        public dynamic Data { get; }
        public GlobalEventArgs(Events.Events eventType, dynamic data)
        {
            EventType = eventType;
            Data = data;
        }
    }
}