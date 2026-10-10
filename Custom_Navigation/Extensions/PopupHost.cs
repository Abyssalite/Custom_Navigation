using CommunityToolkit.Mvvm.ComponentModel;

namespace Custom_Popup;

public partial class PopupHost : ObservableObject, IPopupHost
{
    [ObservableProperty]
    private object? popup;


    private TaskCompletionSource<object?>? _completionSource;
    public bool IsOpen => Popup != null;

    public Task ShowTextPopup(object? vm)
    {
        Show(vm);

        return WaitForCloseAsync();
    }

    public async Task<object?> ShowInputPopup(object? vm)
    {
        Show(vm);

        var result = await WaitForCloseAsync();

        return result;
    }

    public async Task ShowNotifyPopup( object? vm, int sec = 0) 
    { 
        Show(vm); 

        if (sec > 0) 
            _ = AutoCloseAsync(sec); 
        
        await WaitForCloseAsync(); 
    }

    private void Show(object? vm)
    {
        // Don't allow two popups to be displayed
        if (Popup != null)
            throw new InvalidOperationException("A popup is already open.");

        _completionSource =
            new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);

        Popup = vm;

        OnPropertyChanged(nameof(IsOpen));
    }

    private Task<object?> WaitForCloseAsync()
    {
        if (_completionSource == null)
            throw new InvalidOperationException("No popup is currently open.");

        return _completionSource.Task;
    }

    private async Task AutoCloseAsync(int sec) 
    { 
        try 
        { 
            await Task.Delay(TimeSpan.FromSeconds(sec)); 
            // The parent/user may already have closed it. 
            if (IsOpen) Close(); 
        } 
        catch 
        { 
            // Don't allow the background task to 
            // produce an unobserved exception. 
        } 
    }

    public void Close(object? result = null)
    {
        if (!IsOpen) return;

        Popup = null;

        OnPropertyChanged(nameof(IsOpen));

        _completionSource?.TrySetResult(result);
        _completionSource = null;
    }
}