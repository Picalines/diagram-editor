namespace DiagramEditor.Application.UseCases.User.UpdateCurrent;

using DiagramEditor.Application.Repositories;

public enum UpdateCurrentUserError
{
    InvalidLogin = UserUpdateError.InvalidLogin,
    InvalidPassword = UserUpdateError.InvalidPassword,
    LoginTaken = UserUpdateError.LoginTaken,
    Unauthorized,
}
