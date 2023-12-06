namespace DiagramEditor.Application.UseCases.User.GetCurrent;

public interface IGetCurrentUserUseCase : IUseCase<Unit, CurrentUserResponse, EnumError<GetCurrentUserError>>
{
}
