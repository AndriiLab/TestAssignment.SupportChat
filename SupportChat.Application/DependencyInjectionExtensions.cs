using System.Runtime.InteropServices;
using FluentScheduler;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Savorboard.CAP.InMemoryMessageQueue;
using SupportChat.Application.Background;
using SupportChat.Application.Background.EventHandlers;
using SupportChat.Application.Interfaces;
using SupportChat.Application.Services;
using SupportChat.Application.Services.Scheduler;
using SupportChat.Core.Events;

namespace SupportChat.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddCap(o =>
        {
            o.UseInMemoryStorage();
            o.UseInMemoryMessageQueue();
        });

        services
            .AddTransient<ISessionManager, SessionManager>()
            .AddTransient<IAgentCoordinator, AgentCoordinator>()
            .AddTransient<ITeamScheduler, TeamScheduler>();
        
        services.Scan(scan => scan
            .FromAssemblyOf<AgentNotAssignedEventHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(IEventHandler<>)))
            .AsSelf()
            .AddClasses(classes => classes.AssignableTo<IJob>())
            .AsSelf()
            .WithTransientLifetime()
            .AddClasses(classes => classes.AssignableTo<IHostedService>())
            .AsSelfWithInterfaces()
            .WithSingletonLifetime());
        
        return services;
    }
    
    public static IApplicationBuilder UseApplicationLayer(this IApplicationBuilder app)
    {
        app.ConfigureJobs();
        
        return app;
    }

    private static void ConfigureJobs(this IApplicationBuilder app)
    {
        var registry = new Registry();
        registry.Schedule<ScheduleGeneratorStartupJob>().ToRunNow();
        registry.Schedule<ScheduleGeneratorMonthlyJob>().ToRunEvery(1).Months().OnTheLastDay();
        registry.Schedule<SessionInvalidatorJob>().ToRunEvery(1).Seconds();

        JobManager.Initialize(registry);
    }
}