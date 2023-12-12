using CSharpFunctionalExtensions;
using DiagramEditor.Application.Errors;

namespace DiagramEditor.Application.UseCases;

public interface IUseCase<R, S, E>
    where E : IError
{
    public Task<Result<S, E>> Execute(R request);
}
