using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TaskLogix.Events;
using TaskLogix.Factory;
using TaskLogix.Services;

public class EventServiceWorker : IHostedService, IDisposable
{
    private readonly ILogger<EventServiceWorker> _logger;
    private readonly IEventService _eventService;
    private readonly IServiceScopeFactory _serviceFactory;

    public EventServiceWorker(ILogger<EventServiceWorker> logger, IEventService eventService, IServiceScopeFactory serviceFactory)
    {
        _logger = logger;
        _eventService = eventService;
        _serviceFactory = serviceFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("EventListenerService is starting.");

        _eventService.GlobalEventHandler += EventService_EventChecker;

        return Task.CompletedTask;
    }

    private void EventService_EventChecker(object sender, GlobalEventArgs globalEventArgs)
    {
        using (var scope = _serviceFactory.CreateScope())
        {
            var serviceFactory = scope.ServiceProvider.GetRequiredService<IServiceFactory>();
            var service = serviceFactory.GetNotificationService(globalEventArgs.EventType, globalEventArgs);
            service.SendNotificationAsync();
        }

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("EventListenerService is stopping.");

        _eventService.GlobalEventHandler -= EventService_EventChecker;

        return Task.CompletedTask;
    }

    public void Dispose()
    {

    }
}
