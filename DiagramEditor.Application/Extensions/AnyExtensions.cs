namespace DiagramEditor.Application.Extensions;

public static class AnyExtensions
{
    public static void IfNotNull<T>(this T? nullable, Action action)
    {
        if (nullable is { })
        {
            action.Invoke();
        }
    }

    public static void IfNotNull<T>(this T? nullable, Action<T> action)
    {
        if (nullable is { } value)
        {
            action.Invoke(value);
        }
    }
}
