using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UserService.Application.Interfaces;

namespace UserService.Application.Features.Users.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserProfileCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {request.Id} was not found.");
        }

        user.UpdateProfile(request.FirstName, request.LastName);

        await _userRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
