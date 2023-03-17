using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SupportChat.Application.Interfaces;
using SupportChat.Infrastructure.Queues;

namespace SupportChat.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {
        services
            .AddSingleton<IJuniorAgentQueue, AgentQueue>()
            .AddSingleton<IOverflowAgentQueue, AgentQueue>()
            .AddSingleton<IMiddleAgentQueue, AgentQueue>()
            .AddSingleton<ISeniorAgentQueue, AgentQueue>()
            .AddSingleton<ITeamLeadAgentQueue, AgentQueue>()
            .AddSingleton<ISessionQueue, SessionQueue>();
        
        return services;
    }
    
    public static IApplicationBuilder UseInfrastructureLayer(this IApplicationBuilder app)
    {
        return app;
    }
}