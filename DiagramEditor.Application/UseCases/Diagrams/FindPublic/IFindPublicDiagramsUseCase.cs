using DiagramEditor.Application.Errors;

namespace DiagramEditor.Application.UseCases.Diagrams.FindPublic;

public interface IFindPublicDiagramsUseCase
    : IUseCase<
        FindPublicDiagramsRequest,
        FindPublicDiagramsResponse,
        EnumError<FindPublicDiagramsError>
    > { }

