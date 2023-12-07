namespace DiagramEditor.Application.Services;

public interface IValidator<T>
{
    public bool Validate(T value, out IReadOnlyList<string> errors);

    public static bool ValidateAll(
        out IReadOnlyList<string> errors,
        params (IValidator<T> Validator, T Value)[] cases
    )
    {
        var isValid = true;
        var errorList = new List<string>();

        foreach (var (validator, value) in cases)
        {
            isValid = validator.Validate(value, out var innerErrors) && isValid;
            errorList.AddRange(innerErrors);
        }

        errors = errorList;
        return isValid;
    }
}

public static class IValidator
{
    public static bool ValidateAll<T>(
        out IReadOnlyList<string> errors,
        params (IValidator<T> Validator, T Value)[] cases
    )
    {
        return IValidator<T>.ValidateAll(out errors, cases);
    }
}
