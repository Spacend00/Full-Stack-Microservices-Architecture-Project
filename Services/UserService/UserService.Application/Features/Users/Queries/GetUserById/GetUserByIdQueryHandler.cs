using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UserService.Application.Interfaces;

namespace UserService.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdDto?>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetUserByIdDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
        {
            return null;
        }

        return new GetUserByIdDto(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.CreatedAt
        );
    }
}

