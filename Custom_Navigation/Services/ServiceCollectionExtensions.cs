using Microsoft.Extensions.DependencyInjection;

namespace Custom_Navigation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAvaloniaNavigation(this IServiceCollection services)
    {
        services.AddSingleton<IViewHost, ViewHost>();
        services.AddSingleton<INavigatorService, NavigatorService>();
        services.AddSingleton<ITabView, TabView>();

        return services;
    }
}