using DiagramEditor.Application.Errors;

namespace DiagramEditor.Application.UseCases.Diagrams.GetOwned;

public interface IGetOwnedDiagramsUseCase
    : IUseCase<Unit, GetOwnedDiagramsResponse, EnumError<GetOwnedDiagramsError>> { }

