namespace DiagramEditor.Application.UseCases.User.Register;

public sealed record RegisterResponse : IResponse
{
    public required string AccessToken { get; set; }

    public required string RefreshToken { get; set; }
}
