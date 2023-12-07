using DiagramEditor.Domain.Users;

namespace DiagramEditor.Application.UseCases.Users.GetCurrent;

public sealed record CurrentUserResponse(User User) : IResponse;
