namespace Custom_Popup;

public interface IPopupHost
{
    object? Popup { get; }

    bool IsOpen { get; }

    Task ShowTextPopup(object? vm);

    Task<object?> ShowInputPopup(object? vm);

    Task ShowNotifyPopup(object? vm, int sec = 0);

    void Close(object? result = null);
}