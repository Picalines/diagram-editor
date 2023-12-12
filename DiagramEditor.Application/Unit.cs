namespace DiagramEditor.Application;

public sealed class Unit
{
    public static Unit Instance { get; } = new();

    private Unit() { }
}
