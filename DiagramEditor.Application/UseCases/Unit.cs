namespace DiagramEditor.Application.UseCases;

public sealed class Unit : IRequest, IResponse
{
    public static Unit Instance { get; } = new();

    private Unit() { }
}
