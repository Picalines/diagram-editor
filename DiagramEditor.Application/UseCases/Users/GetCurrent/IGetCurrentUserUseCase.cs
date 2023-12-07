namespace DiagramEditor.Application.UseCases.Users.GetCurrent;

public interface IGetCurrentUserUseCase : IUseCase<Unit, CurrentUserResponse, EnumError<GetCurrentUserError>>
{
}
