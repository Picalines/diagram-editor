namespace DiagramEditor.Application.Services.Passwords;

public interface IPasswordValidator
{
    public bool Validate(string passwordText);
}
