namespace DiagramEditor.Application.UseCases;

public interface IError
{
    public string Code { get; }

    public string Message { get; }
}
