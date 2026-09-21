
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Interfaces;

namespace UserService.Application.Features.Users.Queries.GetAllActiveUser
{
    public class GetAllActiveUserQueryHandler : IRequestHandler<GetAllActiveUserQuery, IReadOnlyList<GetAllActiveUserDto>>
    {
        private readonly IUserRepository _userRepository;
        public GetAllActiveUserQueryHandler(IUserRepository userRepository) => _userRepository = userRepository;

        public async Task<IReadOnlyList<GetAllActiveUserDto>> Handle(GetAllActiveUserQuery request, CancellationToken cancellationToken)
        {
            var query = _userRepository.GetAll().Where(u => u.IsActive);

            var users = await query.Select(u => new GetAllActiveUserDto(
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.CreatedAt
            )).ToListAsync(cancellationToken);

            return users;
        }
    }
}
