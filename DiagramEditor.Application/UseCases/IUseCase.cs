using CSharpFunctionalExtensions;

namespace DiagramEditor.Application.UseCases;

public interface IUseCase<R, S, E>
    where R : IRequest
    where S : IResponse
    where E : IError
{
    public Task<Result<S, E>> Execute(R request);
}
