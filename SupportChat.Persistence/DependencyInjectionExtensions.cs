using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SupportChat.Persistence;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services)
    {
        return services;
    }
    
    public static IApplicationBuilder UsePersistenceLayer(this IApplicationBuilder app)
    {
        return app;
    }
}