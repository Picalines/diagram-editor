namespace DiagramEditor.Application.Services.Passwords;

public interface IPasswordHasher
{
    public string Hash(string passwordText);

    public bool Verify(string passwordText, string PasswordHash);
}
