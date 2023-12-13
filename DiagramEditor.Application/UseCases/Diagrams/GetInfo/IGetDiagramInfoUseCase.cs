using DiagramEditor.Application.Errors;

namespace DiagramEditor.Application.UseCases.Diagrams.GetInfo;

public interface IGetDiagramInfoUseCase
    : IUseCase<GetDiagramInfoRequest, GetDiagramInfoResponse, EnumError<GetDiagramInfoError>> { }
