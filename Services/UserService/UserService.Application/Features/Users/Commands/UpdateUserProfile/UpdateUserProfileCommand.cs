using MediatR;

namespace UserService.Application.Features.Users.Commands.UpdateUserProfile;

public record UpdateUserProfileCommand(
    Guid Id,
    string FirstName,
    string LastName
) : IRequest<Unit>;
