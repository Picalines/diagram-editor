using System.ComponentModel;
using System.Reflection;
using CSharpFunctionalExtensions;

namespace DiagramEditor.Application.Extensions;

public static class EnumExtensions
{
    private static readonly Dictionary<Enum, string?> _cachedDescriptions = [];

    public static Maybe<string> GetDescription(this Enum enumValue)
    {
        if (_cachedDescriptions.TryGetValue(enumValue, out var description) is false)
        {
            description = enumValue
                .GetType()
                .GetField(enumValue.ToString())!
                .GetCustomAttribute<DescriptionAttribute>()
                ?.Description;

            _cachedDescriptions.Add(enumValue, description);
        }

        return description.AsMaybe();
    }

    public static int ToInt(this Enum enumValue)
    {
        return (int)(object)enumValue;
    }

    public static T CastTo<T>(this Enum enumValue)
        where T : struct, Enum
    {
        var casted = (T)(object)enumValue.ToInt();

        if (Enum.GetValues<T>().Contains(casted) is false)
        {
            throw new InvalidCastException($"cannot cast value {enumValue} to {typeof(T)}");
        }

        return casted;
    }
}
