namespace DiagramEditor.Application.UseCases.Users.UpdateCurrent;

using DiagramEditor.Application.Repositories;

public enum UpdateCurrentUserError
{
    LoginTaken = UserUpdateError.LoginTaken,
    ValidationError,
    Unauthorized,
}
