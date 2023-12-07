namespace DiagramEditor.Application.UseCases.Users.Register;

public sealed record RegisterResponse : IResponse
{
    public required string AccessToken { get; set; }

    public required string RefreshToken { get; set; }
}
