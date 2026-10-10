using Microsoft.Extensions.DependencyInjection;

using Custom_Navigation;
using Custom_Tab;
using Custom_Popup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomNavigation(this IServiceCollection services)
    {
        services.AddSingleton<IViewHost, ViewHost>();
        services.AddSingleton<INavigatorService, NavigatorService>();
        services.AddSingleton<ITabView, TabView>();
        services.AddSingleton<IPopupHost, PopupHost>();
        return services;
    }
}