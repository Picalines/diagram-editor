using DiagramEditor.Application.Repositories;

namespace DiagramEditor.Application.UseCases.Users.Register;

public enum RegisterError
{
    InvalidLogin = UserCreationError.InvalidLogin,
    InvalidPassword = UserCreationError.InvalidPassword,
    LoginTaken = UserCreationError.LoginTaken,
}
