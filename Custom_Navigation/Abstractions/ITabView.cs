namespace Custom_Navigation;

public interface ITabView
{
    object? MainTab { get; set; }
    object? SecondaryTab { get; set; }

    int TabCount { get; }
    int MainTabIndex { get; }
    int SecondaryTabIndex { get; }

    Task switchMainTab(int index);
    Task switchSecondaryTab(int index);
    Task addTab(object? tab);
    Task clearTab();
}