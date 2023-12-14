using DiagramEditor.Application.Errors;

namespace DiagramEditor.Application.UseCases.Diagrams.UpdateInfo;

public interface IUpdateDiagramInfoUseCase
    : IUseCase<
        UpdateDiagramInfoRequest,
        UpdateDiagramInfoResponse,
        EnumError<UpdateDiagramInfoError>
    > { }
