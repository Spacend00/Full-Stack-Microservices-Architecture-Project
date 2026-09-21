
namespace UserService.Application.Features.Users.Commands.LoginUser
{
    public record LoginUserDto(string AccessToken, string RefreshToken, DateTime RefreshTokenExpireTime);
}
