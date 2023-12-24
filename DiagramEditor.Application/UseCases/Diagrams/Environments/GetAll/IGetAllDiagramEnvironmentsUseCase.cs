using DiagramEditor.Application.Errors;

namespace DiagramEditor.Application.UseCases.Diagrams.Environments.GetAll;

public interface IGetAllDiagramEnvironmentsUseCase
    : IUseCase<
        GetAllDiagramEnvironmentsRequest,
        GetAllDiagramEnvironmentsResponse,
        EnumError<GetAllDiagramEnvironmentsError>
    > { }

