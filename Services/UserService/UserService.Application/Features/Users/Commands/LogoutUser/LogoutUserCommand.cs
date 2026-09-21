
using MediatR;

namespace UserService.Application.Features.Users.Commands.LogoutUser
{
    public record class LogoutUserCommand(string RefreshToken, string AccessToken, DateTime AccessTokenExpiration) : IRequest<bool>;
}
