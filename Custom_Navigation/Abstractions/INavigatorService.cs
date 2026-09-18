namespace Custom_Navigation;

public interface INavigatorService
{
    NavigationState? FirstView { get; set; }

    Task NavigateMain(object? main);
    Task NavigateSide(object? side);
    Task ChangeTopBar(object? topBar);
    Task NavigateMainAndTop(object? main, object? topBar);
    Task Navigate(NavigationState state);
    Task OpenPrevious();

    Task ClearStack();
    bool IsExit();
}