namespace DiagramEditor.Application.Errors;

public sealed record Error(string Code, string Message) : IError;
