using DiagramEditor.Application.Errors;

namespace DiagramEditor.Application.UseCases.Users.UpdateCurrent;

public interface IUpdateCurrentUserUseCase
    : IUseCase<
        UpdateCurrentUserRequest,
        UpdateCurrentUserResponse,
        EnumError<UpdateCurrentUserError>
    > { }
