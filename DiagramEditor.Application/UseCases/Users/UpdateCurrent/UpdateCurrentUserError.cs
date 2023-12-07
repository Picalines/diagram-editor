namespace DiagramEditor.Application.UseCases.Users.UpdateCurrent;

using DiagramEditor.Application.Repositories;

public enum UpdateCurrentUserError
{
    InvalidLogin = UserUpdateError.InvalidLogin,
    InvalidPassword = UserUpdateError.InvalidPassword,
    LoginTaken = UserUpdateError.LoginTaken,
    Unauthorized,
}
