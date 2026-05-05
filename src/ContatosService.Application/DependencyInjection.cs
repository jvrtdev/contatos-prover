using ContatosService.Application.Interfaces;
using ContatosService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ContatosService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IContatoService, ContatoService>();

        return services;
    }
}
