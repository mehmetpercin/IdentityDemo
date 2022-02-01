using IdentityDemo.Dtos;
using IdentityDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityDemo.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly TokenOption _tokenOption;
        public AuthenticationService(UserManager<AppUser> userManager, AppDbContext dbContext, IOptions<TokenOption> options)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _tokenOption = options.Value;
        }

        public async Task<Response<TokenDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return Response<TokenDto>.Fail("Email or Password is wrong", 400);

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Response<TokenDto>.Fail("Email or Password is wrong", 400);
            }
            var token = await CreateToken(user);

            var userRefreshToken = await _dbContext.UserRefreshTokens.FirstOrDefaultAsync(x => x.UserId == user.Id);

            if (userRefreshToken == null)
            {
                await _dbContext.UserRefreshTokens.AddAsync(new UserRefreshToken { UserId = user.Id, RefreshToken = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
            }
            else
            {
                userRefreshToken.RefreshToken = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _dbContext.SaveChangesAsync();

            return Response<TokenDto>.Success(token, 200);
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _dbContext.UserRefreshTokens.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

            if (existRefreshToken == null)
            {
                return Response<TokenDto>.Fail("Refresh token not found", 404);
            }

            var user = await _userManager.FindByIdAsync(existRefreshToken.UserId.ToString());

            if (user == null)
            {
                return Response<TokenDto>.Fail("User Id not found", 404);
            }

            var tokenDto = await CreateToken(user);

            existRefreshToken.RefreshToken = tokenDto.RefreshToken;
            existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            await _dbContext.SaveChangesAsync();

            return Response<TokenDto>.Success(tokenDto, 200);
        }

        public async Task<Response<object>> RevokeRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _dbContext.UserRefreshTokens.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (existRefreshToken == null)
            {
                return Response<object>.Fail("Refresh token not found", 404);
            }

            _dbContext.UserRefreshTokens.Remove(existRefreshToken);

            await _dbContext.SaveChangesAsync();

            return Response<object>.Success(200);
        }

        private async Task<TokenDto> CreateToken(AppUser appUser)
        {
            var accessTokenExpiration = DateTime.Now.AddDays(_tokenOption.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.RefreshTokenExpiration);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOption.SecurityKey));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenOption.Issuer,
                expires: accessTokenExpiration,
                audience: _tokenOption.Audience,
                notBefore: DateTime.Now,
                claims: await GetClaims(appUser),
                signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };

            return tokenDto;
        }

        private async Task<IEnumerable<Claim>> GetClaims(AppUser appUser)
        {
            var userClaims = await _userManager.GetClaimsAsync(appUser);
            var roles = await _userManager.GetRolesAsync(appUser);
            var roleClaims = new List<Claim>();
            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, appUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
                new Claim("uid", appUser.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            return claims;
        }

        private string CreateRefreshToken()
        {
            var numberByte = new byte[32];

            using var rnd = RandomNumberGenerator.Create();

            rnd.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);
        }
    }
}
