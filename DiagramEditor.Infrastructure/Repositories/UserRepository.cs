using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure.Repositories;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class UserRepository(ApplicationContext context) : IUserRepository
{
    public void Add(User user)
    {
        context.Users.Attach(user);
        context.SaveChanges();
    }

    public Maybe<User> GetById(Guid userId)
    {
        return context.Users.SingleOrDefault(user => user.Id == userId).AsMaybe();
    }

    public Maybe<User> GetByLogin(string login)
    {
        return context.Users.SingleOrDefault(user => user.Login == login).AsMaybe();
    }

    public void Update(User user)
    {
        context.Users.Update(user);
        context.SaveChanges();
    }
}
