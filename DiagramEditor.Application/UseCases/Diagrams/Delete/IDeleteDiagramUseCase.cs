using DiagramEditor.Application.Errors;

namespace DiagramEditor.Application.UseCases.Diagrams.Delete;

public interface IDeleteDiagramUseCase
    : IUseCase<DeleteDiagramRequest, Unit, EnumError<DeleteDiagramError>> { }
