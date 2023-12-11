using CSharpFunctionalExtensions;

namespace DiagramEditor.Application.UseCases.Users.UpdateCurrent;

public sealed record UpdateCurrentUserRequest : IRequest
{
    public Maybe<string> Login { get; init; }

    public Maybe<string> Password { get; init; }

    public Maybe<string> DisplayName { get; init; }

    public Maybe<string> AvatarUrl { get; init; }
}
