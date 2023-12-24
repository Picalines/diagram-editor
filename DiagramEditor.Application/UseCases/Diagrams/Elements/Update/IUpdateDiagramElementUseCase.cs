using DiagramEditor.Application.Errors;

namespace DiagramEditor.Application.UseCases.Diagrams.Elements.UseCase;

public interface IUpdateDiagramElementUseCase
    : IUseCase<
        UpdateDiagramElementRequest,
        UpdateDiagramElementResponse,
        EnumError<UpdateDiagramElementError>
    > { }

