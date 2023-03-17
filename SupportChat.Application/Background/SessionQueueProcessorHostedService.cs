using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Polly;
using SupportChat.Application.Events;
using SupportChat.Application.Interfaces;
using SupportChat.Core.Exceptions;

namespace SupportChat.Application.Background;

public class SessionQueueProcessorHostedService : IHostedService
{
    private const int WaitSeconds = 5;
    private readonly IServiceProvider _serviceProvider;
    private Task? _dequeueTask;

    public SessionQueueProcessorHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _dequeueTask = DequeueAsync();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAny(_dequeueTask ?? Task.CompletedTask, Task.Delay(Timeout.Infinite, CancellationToken.None));
    }
    
    private Task? DequeueAsync()
    {
        var policy = Policy
            .Handle<Exception>()
            .WaitAndRetryForeverAsync(
                _ => TimeSpan.FromSeconds(WaitSeconds),
                (exception, _) => throw new SupportChatException(exception.Message));

        return policy.ExecuteAsync(async () =>
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var sessionManager = serviceScope.ServiceProvider.GetRequiredService<ISessionManager>();
            var sessionId = sessionManager.GetNextUnassignedSessionId();
            if (sessionId is null)
            {
                await Task.Delay(TimeSpan.FromSeconds(WaitSeconds));
                return;
            }
            
            var agentCoordinator = serviceScope.ServiceProvider.GetRequiredService<IAgentCoordinator>();
            var agentId = agentCoordinator.GetNextAgentId();
            if (agentId is null)
            {
                var bus = serviceScope.ServiceProvider.GetRequiredService<ICapPublisher>();
                await bus.PublishAsync(SessionNotAssignedEvent.Key, new SessionNotAssignedEvent(sessionId.Value));
                await Task.Delay(TimeSpan.FromSeconds(WaitSeconds));
                return;
            }

            var db = serviceScope.ServiceProvider.GetRequiredService<IDatabase>();
            var session = db.Sessions.FirstOrDefault(s => s.Id == sessionId && s.IsActive);
            if (session is null)
            {
                var bus = serviceScope.ServiceProvider.GetRequiredService<ICapPublisher>();
                await bus.PublishAsync(AgentNotAssignedEvent.Key, new AgentNotAssignedEvent(agentId.Value));
                return;
            }

            session.AgentId = agentId;
        });
    }
}