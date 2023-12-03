using CSharpFunctionalExtensions;

namespace DiagramEditor.Common.Extensions;

public static class IParsableExtensions
{
    public static Maybe<T> MaybeParse<T>(
        this string s,
        IFormatProvider? formatProvider = null
    )
        where T : IParsable<T>
    {
        if (T.TryParse(s, formatProvider, out var value) is false)
        {
            return Maybe.None;
        }

        return value;
    }
}
