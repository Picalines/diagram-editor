using DiagramEditor.Application.Errors;

namespace DiagramEditor.Application.UseCases.Authentication.Logout;

public interface ILogoutUseCase : IUseCase<Unit, Unit, EnumError<LogoutError>> { }
