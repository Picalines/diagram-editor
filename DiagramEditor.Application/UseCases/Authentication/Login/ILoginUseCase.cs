namespace DiagramEditor.Application.UseCases.Authentication.Login;

public interface ILoginUseCase : IUseCase<LoginRequest, LoginResponse, EnumError<LoginError>>
{
}
