using DiagramEditor.Application.Repositories;

namespace DiagramEditor.Application.UseCases.Users.Register;

public enum RegisterError
{
    LoginTaken = UserCreationError.LoginTaken,
    ValidationError,
}
