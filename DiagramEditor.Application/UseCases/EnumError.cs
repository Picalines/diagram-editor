using CSharpFunctionalExtensions;
using DiagramEditor.Application.Extensions;

namespace DiagramEditor.Application.UseCases;

public sealed class EnumError<TEnum>(TEnum error) : IError
    where TEnum : Enum
{
    public string Message => error.GetDescription().GetValueOrDefault(error.ToString);

    public static implicit operator EnumError<TEnum>(TEnum error)
    {
        return new EnumError<TEnum>(error);
    }
}

public static class EnumError
{
    public static EnumError<TEnum> Of<TEnum>(TEnum error)
        where TEnum : Enum
    {
        return new EnumError<TEnum>(error);
    }
}
