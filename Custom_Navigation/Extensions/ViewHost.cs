using CommunityToolkit.Mvvm.ComponentModel;

namespace Custom_Navigation;

public partial class ViewHost : ObservableObject, IViewHost
{
    [ObservableProperty]
    private object? sideView;

    [ObservableProperty]
    private object? mainView;

    [ObservableProperty]
    private object? topBar;

    public Task NavigateSide(object? content)
    {
        SideView = content;
        return Task.CompletedTask;
    }

    public Task NavigateMain(object? content)
    {
        MainView = content;
        return Task.CompletedTask;
    }

    public Task ChangeTopBar(object? content)
    {
        TopBar = content;
        return Task.CompletedTask;
    }
}