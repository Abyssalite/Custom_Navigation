namespace Custom_Navigation;

public interface IViewHost
{
    object? SideView { get; set; }
    object? MainView { get; set; }
    object? TopBar { get; set; }

    Task NavigateSide(object? content);
    Task NavigateMain(object? content);
    Task ChangeTopBar(object? content);
}