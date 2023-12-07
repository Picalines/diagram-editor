namespace DiagramEditor.Application.UseCases.User.UpdateCurrent;
using DiagramEditor.Domain.Users;

public sealed record UpdateCurrentUserResponse(User User) : IResponse;
