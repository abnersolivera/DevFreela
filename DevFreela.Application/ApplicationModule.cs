using DevFreela.Application.Commands.InsertComment;
using Microsoft.Extensions.DependencyInjection;

namespace DevFreela.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddHandlers();
        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblyContaining<InsertCommentCommand>();
        });
        return services;
    }
}