namespace DiagramEditor.Application.UseCases.Authentication.Refresh;

public interface IRefreshUseCase
    : IUseCase<RefreshRequest, RefreshResponse, EnumError<RefreshError>> { }
