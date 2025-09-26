using Api.Models.Dtos.Requests;
using Api.Models.Dtos.Responses;

public interface IAuthService
{
    AuthUserInfo Authenticate(LoginRequest request);
    Task<AuthUserInfo> Register(RegisterRequest request);
}