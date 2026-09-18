namespace Custom_Navigation;

public sealed record NavigationState(
    object? MainContent,
    object? SideContent,
    object? TopBar = null
);