using MediatR;
using UserService.Application.Interfaces;

namespace UserService.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null) throw new KeyNotFoundException($"User with ID {request.Id} was not found.");

        user.Deactivate();
        await _userRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
