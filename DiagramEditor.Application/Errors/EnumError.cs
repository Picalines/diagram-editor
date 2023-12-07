using CSharpFunctionalExtensions;
using DiagramEditor.Application.Extensions;

namespace DiagramEditor.Application.Errors;

public sealed class EnumError<TEnum>(TEnum error, IReadOnlyList<string> details) : IError
    where TEnum : struct, Enum
{
    public EnumError(TEnum error, params string[] details)
        : this(error, (IReadOnlyList<string>)details) { }

    public EnumError(TEnum error)
        : this(error, []) { }

    public TEnum Error => error;

    public string Code => error.ToString();

    public string Message => error.GetDescription().GetValueOrDefault(error.ToString);

    public IReadOnlyList<string> Details { get; } = details;

    public static implicit operator EnumError<TEnum>(TEnum error)
    {
        return new EnumError<TEnum>(error);
    }
}

public static class EnumError
{
    public static EnumError<TEnum> From<TEnum>(TEnum error, IReadOnlyList<string> details)
        where TEnum : struct, Enum
    {
        return new EnumError<TEnum>(error, details);
    }

    public static EnumError<TEnum> From<TEnum>(TEnum error, params string[] details)
        where TEnum : struct, Enum
    {
        return From(error, (IReadOnlyList<string>)details);
    }

    public static EnumError<TEnum> From<TEnum>(TEnum error)
        where TEnum : struct, Enum
    {
        return From(error, []);
    }
}
