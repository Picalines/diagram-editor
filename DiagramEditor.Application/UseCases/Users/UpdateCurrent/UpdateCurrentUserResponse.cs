using DiagramEditor.Domain.Users;

namespace DiagramEditor.Application.UseCases.Users.UpdateCurrent;

public sealed record UpdateCurrentUserResponse(User User) : IResponse;
