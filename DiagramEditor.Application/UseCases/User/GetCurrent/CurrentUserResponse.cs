namespace DiagramEditor.Application.UseCases.User.GetCurrent;

using DiagramEditor.Domain.Users;

public sealed record CurrentUserResponse(User User) : IResponse;
