namespace DiagramEditor.Application.UseCases;

public sealed record Error(string Message) : IError;
