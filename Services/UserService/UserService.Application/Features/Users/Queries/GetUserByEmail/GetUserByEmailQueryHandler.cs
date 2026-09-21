using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UserService.Application.Interfaces;

namespace UserService.Application.Features.Users.Queries.GetUserByEmail;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, GetUserByEmailDto?>
{
    private readonly IUserRepository _userRepository;

    public GetUserByEmailQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetUserByEmailDto?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            return null;
        }

        return new GetUserByEmailDto(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.CreatedAt
        );
    }
}
