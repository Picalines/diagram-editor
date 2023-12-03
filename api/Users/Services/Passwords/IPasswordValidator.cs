namespace DiagramEditor.Services.Passwords;

public interface IPasswordValidator
{
    public bool Validate(string passwordText);
}
