using DiagramEditor.Application.Errors;

namespace DiagramEditor.Application.UseCases.Diagrams.Environments.Create;

public interface ICreateDiagramEnvironmentUseCase
    : IUseCase<
        CreateDiagramEnvironmentRequest,
        CreateDiagramEnvironmentResponse,
        EnumError<CreateDiagramEnvironmentError>
    > { }

