namespace DiagramEditor.Application.UseCases;

public sealed record Error(string Code, string Message) : IError;
