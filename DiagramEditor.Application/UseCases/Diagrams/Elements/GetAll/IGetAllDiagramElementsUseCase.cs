using DiagramEditor.Application.Errors;

namespace DiagramEditor.Application.UseCases.Diagrams.Elements.GetAll;

public interface IGetAllDiagramElementsUseCase
    : IUseCase<
        GetAllDiagramElementsRequest,
        GetAllDiagramElementsResponse,
        EnumError<GetAllDiagramElementsError>
    > { }

