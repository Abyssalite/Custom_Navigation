# Custom_Navigation

A lightweight, MVVM-friendly **navigation system** designed for **Avalonia** applications.

It provides a clean separation between navigation logic and UI by managing:

- Main content
- Side panel / drawer
- Top bar
- Navigation history (back stack)
- Popups
- Simple tab management

Perfect for desktop Avalonia apps that need structured navigation without heavy frameworks.

## Features

- **Three-region layout**
  - `MainView`
  - `SideView`
  - `TopBar`

- **Navigation stack** with back support
- **Custom back handling** via `IHandleBackNavigation`
- **Last page callback** via `IHandleLastPage`
- **Popup system** (text, input, auto-closing notify)
- **Simple tab management**
- **Fully DI ready** (`AddCustomNavigation()`)
- Built with `CommunityToolkit.Mvvm` + `Microsoft.Extensions.DependencyInjection`
- Targets **.NET 10**


## Installation

Add the project as a reference or pack it as a NuGet package.

### Dependency Injection (Avalonia)

In your `Program.cs` / `App.axaml.cs` (or wherever you configure services):

```csharp
using Custom_Navigation;

services.AddCustomNavigation();
```

#### This registers:

| Interface | Implementation | Lifetime |
| --- | --- | --- |
| IViewHost | ViewHost | Singleton |
| INavigatorService | NavigatorService | Singleton |
| ITabView | TabView| Singleton |
| IPopupHost | PopupHost | Singleton |

## Core Concepts

#### NavigationState

```csharp
public sealed record NavigationState(
    object? MainContent,
    object? SideContent,
    object? TopBar = null
);
```

#### **IViewHost** (UI binding target)

- Bind these properties in your Avalonia main window / shell:XML

```XML
<!-- Example Avalonia XAML -->
<Grid>
    <ContentControl Content="{Binding TopBar}" />
    <ContentControl Content="{Binding SideView}" />
    <ContentControl Content="{Binding MainView}" />
</Grid>
```

- `ViewHost` is an `ObservableObject`, so it works perfectly with Avalonia + CommunityToolkit.Mvvm.

## Usage Examples

### 1. Basic Navigation

```csharp
public class MainViewModel
{
    private readonly INavigatorService _nav;

    public MainViewModel(INavigatorService nav)
    {
        _nav = nav;

        // Set the root / first page
        _nav.FirstView = new NavigationState(
            mainContent: new HomeViewModel(),
            sideContent: new MenuViewModel(),
            topBar: new DefaultTopBarViewModel()
        );

        // Navigate to first view
        _ = _nav.ClearStack(); // or Navigate(...)
    }

    public async Task GoToSettings()
    {
        await _nav.NavigateMain(new SettingsViewModel());
    }

    public async Task GoToSettingsWithTopBar()
    {
        await _nav.NavigateMainAndTop(
            new SettingsViewModel(),
            new SettingsTopBarViewModel()
        );
    }
}
```
### 2. Back Navigation

```csharp
// In shell or window key handler
await _navigator.OpenPrevious();
```

#### Custom back handling (optional)

```csharp
public class SettingsViewModel : IHandleBackNavigation
{
    public async Task<bool> HandleBackAsync()
    {
        // return true → stop navigation (you handled it)
        // return false → let the navigator go back normally

        if (HasUnsavedChanges)
        {
            // show confirmation, etc.
            return true;
        }

        return false;
    }
}
```

#### Last page callback

```csharp
public class HomeViewModel : IHandleLastPage
{
    public Task HandleLastPageAsync()
    {
        // Called when user tries to go back from the root
        return Task.CompletedTask;
    }
}
```

### 3. Popups

```csharp
// Simple text / info popup
await _popup.ShowTextPopup(new InfoPopupViewModel("Hello!"));

// Input popup (returns a result)
var result = await _popup.ShowInputPopup(new InputPopupViewModel());

// Auto-closing notification
await _popup.ShowNotifyPopup(new ToastViewModel("Saved!"), sec: 3);

// Close from inside the popup
_popup.Close(result: someValue);
```

- Bind the popup in XAML:XML

```XML
<!-- Example Avalonia XAML -->
<ContentControl Content="{Binding Popup}" IsVisible="{Binding IsOpen}" />
```

### 4. Tabs

```csharp
await _tabs.addTab(new Tab1ViewModel());
await _tabs.addTab(new Tab2ViewModel());

await _tabs.switchMainTab(1);
await _tabs.switchSecondaryTab(2);

await _tabs.clearTab(); // usually called automatically on back navigation
```

## Integration Tips

1. Main Window
- Create a `View` + `ViewModel` that exposes:

```csharp
public IViewHost Host { get; }
public IPopupHost Popup { get; }
public ITabView Tabs { get; }
```

2. Data Templates
- Use Avalonia’s `DataTemplate` (or `ViewLocator`) so that ViewModels automatically resolve to their Views.

3. Back button / Escape key
- Hook into a key or a toolbar button and call:

```csharp
await navigator.OpenPrevious();
```

4. Back button / Escape key

```csharp
if (navigator.IsExit())
{
    // user pressed back on the root page twice → close app
    desktop.Shutdown();
}
```

## API Summary

- **INavigatorService**

| Interface | Implementation |
| --- | --- |
| Navigate(NavigationState) | Full navigation |
| NavigateMain(object?) | Change only main content |
| NavigateSide(object?) | Change only side content |
| ChangeTopBar(object?) | Change only top bar |
| NavigateMainAndTop(...) | Change main + top bar |
| OpenPrevious() | Go back |
| ClearStack() | Reset to FirstView |
| IsExit() | Detect double-back on root |

- **IPopupHost**

| Interface | Implementation |
| --- | --- |
| ShowTextPopup | Fire-and-forget style popup |
| ShowInputPopup | Returns a result |
| ShowNotifyPopup | Optional auto-close |
| Close(object? result) | Close current popup |

## License
- This project is currently unlicensed.