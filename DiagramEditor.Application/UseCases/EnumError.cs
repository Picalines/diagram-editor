using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using DiagramEditor.Application.Extensions;

namespace DiagramEditor.Application.UseCases;

public sealed class EnumError<TEnum>(TEnum error) : IError
    where TEnum : Enum
{
    [JsonIgnore]
    public TEnum Error => error;

    public string Code => error.ToString();

    public string Message => error.GetDescription().GetValueOrDefault(error.ToString);

    public static implicit operator EnumError<TEnum>(TEnum error)
    {
        return new EnumError<TEnum>(error);
    }
}

public static class EnumError
{
    public static EnumError<TEnum> From<TEnum>(TEnum error)
        where TEnum : Enum
    {
        return new EnumError<TEnum>(error);
    }
}
