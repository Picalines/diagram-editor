using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using CSharpFunctionalExtensions;

namespace DiagramEditor.Web.API.Configuration;

internal sealed class MaybeJsonConverter : JsonConverterFactory
{
    private static bool IsGenericMaybe(Type typeToConvert)
    {
        return typeToConvert.IsGenericType
            && typeToConvert.GetGenericTypeDefinition() == typeof(Maybe<>);
    }

    public override bool CanConvert(Type typeToConvert)
    {
        return IsGenericMaybe(typeToConvert);
    }

    public override JsonConverter? CreateConverter(
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var innerType = typeToConvert.GetGenericArguments()[0];

        var converter = (JsonConverter?)
            Activator.CreateInstance(
                typeof(MaybeJsonConverterInner<>).MakeGenericType(innerType),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { options },
                culture: null
            );

        return converter;
    }

    public static void Modifier(JsonTypeInfo typeInfo)
    {
        foreach (var propertyInfo in typeInfo.Properties)
        {
            var propertyType = propertyInfo.PropertyType;
            if (!IsGenericMaybe(propertyType))
            {
                continue;
            }

            var hasValueGetter =
                propertyType.GetProperty(nameof(Maybe<object>.HasValue))?.GetMethod
                ?? throw new UnreachableException(
                    $"{nameof(Maybe)}.{nameof(Maybe<object>.HasValue)} getter not found"
                );

            var shouldSerialize = propertyInfo.ShouldSerialize ?? ((_, _) => true);

            propertyInfo.ShouldSerialize = (obj, value) =>
                shouldSerialize(obj, value)
                && hasValueGetter.Invoke(value, Array.Empty<object>()) is true;

            propertyInfo.IsRequired = false;
        }
    }

    private sealed class MaybeJsonConverterInner<T> : JsonConverter<Maybe<T?>>
    {
        private readonly JsonConverter<T> _valueConverter;

        public MaybeJsonConverterInner(JsonSerializerOptions options)
        {
            _valueConverter =
                options.GetConverter(typeof(T)) as JsonConverter<T>
                ?? throw new NotImplementedException(
                    $"{nameof(JsonConverter<T>)} was not found for {nameof(MaybeJsonConverter)}"
                );
        }

        public override Maybe<T?> Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            return reader.TokenType switch
            {
                JsonTokenType.Null => Maybe.None,
                _ => Maybe.From(_valueConverter.Read(ref reader, typeof(T), options)),
            };
        }

        public override void Write(
            Utf8JsonWriter writer,
            Maybe<T?> value,
            JsonSerializerOptions options
        )
        {
            if (value.TryGetValue(out var innerValue) is false)
            {
                throw new UnreachableException(
                    $"should be handled my {nameof(MaybeJsonConverter)}.{nameof(MaybeJsonConverter.Modifier)}"
                );
            }

            _valueConverter.Write(writer, innerValue, options);
        }
    }
}
