using DiagramEditor.Application.Errors;

namespace DiagramEditor.Application.UseCases.Users.Register;

public interface IRegisterUseCase
    : IUseCase<RegisterRequest, RegisterResponse, EnumError<RegisterError>> { }
