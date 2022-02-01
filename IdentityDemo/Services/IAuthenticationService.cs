using IdentityDemo.Dtos;
using IdentityDemo.Models;

namespace IdentityDemo.Services
{
    public interface IAuthenticationService
    {
        Task<Response<TokenDto>> LoginAsync(LoginDto loginDto);
        Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
        Task<Response<object>> RevokeRefreshToken(string refreshToken);
    }
}
