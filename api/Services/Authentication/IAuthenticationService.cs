using DiagramEditor.Database.Models;

namespace DiagramEditor.Services.Authentication;

public interface IAuthenticationService
{
    public string GenerateToken(User user);
}

