using DiagramEditor.Application.Errors;

namespace DiagramEditor.Application.UseCases.Diagrams.Elements.Create;

public interface ICreateDiagramElementUseCase
    : IUseCase<
        CreateDiagramElementRequest,
        CreateDiagramElementResponse,
        EnumError<CreateDiagramElementError>
    > { }

