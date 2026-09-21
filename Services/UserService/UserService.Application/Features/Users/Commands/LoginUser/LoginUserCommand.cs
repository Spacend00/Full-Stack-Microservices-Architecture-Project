
using MediatR;

namespace UserService.Application.Features.Users.Commands.LoginUser
{
    public record LoginUserCommand(string Email, string Password) : IRequest<LoginUserDto>;
}
