namespace Custom_Navigation;

public class NavigatorService : INavigatorService
{
    private readonly IViewHost _host;
    private readonly ITabView _tabs;

    private readonly Stack<NavigationState> _history = new();
    private NavigationState? _current;
    private bool _isExit;

    public NavigationState? FirstView { get; set; }

    public NavigatorService(IViewHost host, ITabView tabs)
    {
        _host = host;
        _tabs = tabs;
    }

    private async void switchPage(object? TopBar, object? SideContent, object? MainContent, NavigationState? last)
    {   
        _isExit = false;

        if (TopBar is not null && !TopBar.Equals(last?.TopBar))
            await _host.ChangeTopBar(TopBar);

        if (SideContent is not null && !SideContent.Equals(last?.SideContent))
            await _host.NavigateSide(SideContent);

        if (MainContent is not null && !MainContent.Equals(last?.MainContent))
            await _host.NavigateMain(MainContent);
    }

    public async Task Navigate(NavigationState state)
    {
        NavigationState? last = _current;
        if (_current is not null)
            _history.Push(_current);

        _current = state;

        switchPage(_current.TopBar, _current.SideContent, _current.MainContent, last);
    }

    public Task NavigateMain(object? main) =>
        Navigate(new NavigationState(
            main,
            _current?.SideContent,
            _current?.TopBar
        ));

    public Task NavigateSide(object? side) =>
        Navigate(new NavigationState(
            _current?.MainContent,
            side,
            _current?.TopBar
        ));

    public Task ChangeTopBar(object? topBar) =>
        Navigate(new NavigationState(
            _current?.MainContent,
            _current?.SideContent,
            topBar
        ));

    public Task NavigateMainAndTop(object? main, object? topBar) =>
        Navigate(new NavigationState(
            main,
            _current?.SideContent,
            topBar
        ));

    public async Task OpenPrevious()
    {
        if (_current is null)
            throw new Exception("Cannot Back on empty page");

        if (_current.MainContent is IHandleBackNavigation backHandler)
        {
            var handled = await backHandler.HandleBackAsync();
            if (handled)
                return;
        }

        if (_history.Count > 0)
        {
            NavigationState? last = _current;

            if (_current.MainContent is IHandleLastPage lastPage && _history.Count == 0)
                await lastPage.HandleLastPageAsync();

            _current = _history.Pop();

            switchPage(_current.TopBar, _current.SideContent, _current.MainContent, last);
            await _tabs.clearTab();
        }
    }

    public async Task ClearStack()
    {
        _history.Clear();

        if (FirstView is null)
            throw new Exception("First view was not set");

        NavigationState? last = _current;
        _current = new NavigationState(
            FirstView.MainContent,
            _current?.SideContent,
            FirstView.TopBar
        );

        switchPage(_current.TopBar, _current.SideContent, _current.MainContent, last);
    }

    public bool IsExit()
    {
        if (_isExit)
            return true;

        if (_history.Count == 0)
        {
            _isExit = true;
            return false;
        }

        return false;
    }
}