namespace Custom_Navigation;

public interface IHandleLastPage
{
    /// <summary>
    /// Trigger a task when the nagivation reach the last page in stack
    /// </summary>
    Task HandleLastPageAsync();
}