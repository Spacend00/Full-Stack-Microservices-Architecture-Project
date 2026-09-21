
using MediatR;
using UserService.Application.Features.Users.Commands.LoginUser;

namespace UserService.Application.Features.Users.Commands.RefreshToken
{
    public record class RefreshTokenCommand(string RefreshToken) : IRequest<LoginUserDto>;
}
