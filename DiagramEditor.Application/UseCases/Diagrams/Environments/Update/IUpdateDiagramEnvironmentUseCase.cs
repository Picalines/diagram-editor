using DiagramEditor.Application.Errors;

namespace DiagramEditor.Application.UseCases.Diagrams.Environments.Update;

public interface IUpdateDiagramEnvironmentUseCase
    : IUseCase<
        UpdateDiagramEnvironmentRequest,
        UpdateDiagramEnvironmentResponse,
        EnumError<UpdateDiagramEnvironmentError>
    > { }

