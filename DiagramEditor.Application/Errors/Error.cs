namespace DiagramEditor.Application.Errors;

public sealed record Error(string Code, string Message, IReadOnlyList<string> Details) : IError
{
    public Error(string code, string message, params string[] details)
        : this(code, message, (IReadOnlyList<string>)details) { }

    public Error(string code, string message)
        : this(code, message, []) { }
}
