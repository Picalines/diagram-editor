using DiagramEditor.Application.Errors;

namespace DiagramEditor.Application.UseCases.Diagrams.Create;

public interface ICreateDiagramUseCase
    : IUseCase<CreateDiagramRequest, CreateDiagramResponse, EnumError<CreateDiagramError>> { }
