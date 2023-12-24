using DiagramEditor.Application.Errors;

namespace DiagramEditor.Application.UseCases.Diagrams.Elements.Delete;

public interface IDeleteDiagramElementUseCase
    : IUseCase<DeleteDiagramElementRequest, Unit, EnumError<DeleteDiagramElementError>> { }

