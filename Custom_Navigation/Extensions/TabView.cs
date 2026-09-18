using CommunityToolkit.Mvvm.ComponentModel;

namespace Custom_Navigation;

public partial class TabView : ObservableObject, ITabView
{
    private Dictionary<int, object>? tabViewMap = new();

    [ObservableProperty]
    private object? mainTab;
    [ObservableProperty]
    private object? secondaryTab;
    [ObservableProperty]
    private int tabCount = 0;
    [ObservableProperty]
    private int mainTabIndex = 0;    
    [ObservableProperty]
    private int secondaryTabIndex = 0;

    public async Task switchMainTab(int index)
    {   
        if (tabViewMap is not null)
        {
            MainTab = tabViewMap[index];
            MainTabIndex = index;
        }
    }
    public async Task switchSecondaryTab(int index)
    {   
        if (tabViewMap is not null)
        {
            SecondaryTab = tabViewMap[index];
            SecondaryTabIndex = index;
        }
    }

    public async Task addTab(object? tab)
    {   
        if (tabViewMap is not null && tab is not null)
        {            
            TabCount++;
            tabViewMap.Add(TabCount, tab);
        }
    }

    public async Task clearTab()
    {   
        if (tabViewMap is not null)
        {
            tabViewMap.Clear();
            TabCount = 0;
        }
    }
}