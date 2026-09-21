using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Interfaces;

namespace UserService.Application.Features.Users.Queries.GetAllUser;

public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, IReadOnlyList<GetAllUserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyList<GetAllUserDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var query = _userRepository.GetAll();

        var users = await query.Select(u => new GetAllUserDto(
            u.Id,
            u.FirstName,
            u.LastName,
            u.Email,
            u.IsActive,
            u.CreatedAt
        )).ToListAsync(cancellationToken);

        return users;
    }
}
