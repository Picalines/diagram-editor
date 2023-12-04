using CSharpFunctionalExtensions;

namespace DiagramEditor.Application.Extensions;

public static class IParsableExtensions
{
    public static Maybe<T> MaybeParse<T>(this string s, IFormatProvider? formatProvider = null)
        where T : IParsable<T>
    {
        return T.TryParse(s, formatProvider, out var value) ? value : Maybe.None;
    }
}