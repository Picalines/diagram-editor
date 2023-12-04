namespace DiagramEditor.Application.Extensions;

public static class StringExtensions
{
    public static string Capitalize(this string str)
    {
        return str.Length is 0 ? "" : char.ToUpper(str[0]) + str[1..];
    }

    public static string Decapitalize(this string str)
    {
        return str.Length is 0 ? "" : char.ToLower(str[0]) + str[1..];
    }

    public static string AddPrefix(this string str, string prefix)
    {
        return str.StartsWith(prefix) ? str : prefix + str;
    }

    public static string RemovePrefix(this string str, string prefix)
    {
        return str.StartsWith(prefix) ? str[prefix.Length..] : str;
    }

    public static string AddSuffix(this string str, string suffix)

    {
        return str.EndsWith(suffix) ? str : str + suffix;
    }

    public static string RemoveSuffix(this string str, string suffix)
    {
        return str.EndsWith(suffix) ? str[..^suffix.Length] : str;
    }
}
