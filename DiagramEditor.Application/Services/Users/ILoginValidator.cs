namespace DiagramEditor.Application.Services.Users;

public interface ILoginValidator
{
    public bool Validate(string login);
}
