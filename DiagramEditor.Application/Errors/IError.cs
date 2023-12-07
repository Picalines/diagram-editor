namespace DiagramEditor.Application.Errors;

public interface IError
{
    public string Code { get; }

    public string Message { get; }

    public IReadOnlyList<string> Details { get; }
}
