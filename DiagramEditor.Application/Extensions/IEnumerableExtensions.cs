namespace DiagramEditor.Application.Extensions;

public static class IEnumerableExtensions
{
    public static IEnumerable<(int Index, T Value)> Indexed<T>(
        this IEnumerable<T> enumerable,
        int firstIndex = 0
    )
    {
        int index = firstIndex;

        foreach (var value in enumerable)
        {
            yield return (index++, value);
        }
    }

    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var value in enumerable)
        {
            action(value);
        }
    }

    public static void ForEach<T, D>(this IEnumerable<T> enumerable, Func<T, D> action)
    {
        foreach (var value in enumerable)
        {
            _ = action(value);
        }
    }
}
