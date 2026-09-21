using System;
using MediatR;

namespace UserService.Application.Features.Users.Commands.UpdatePassword;

public record UpdatePasswordCommand(
    Guid Id,
    string NewPassword
) : IRequest<Unit>;
