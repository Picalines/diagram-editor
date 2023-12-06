namespace DiagramEditor.Application.Extensions;

public static class TaskExtensions
{
    public static Task<T> ToCompletedTask<T>(this T value)
    {
        return Task.FromResult(value);
    }
}
