using MediatR;

namespace UserService.Application.Features.Users.Queries.GetAllActiveUser
{
    public class GetAllActiveUserQuery : IRequest<IReadOnlyList<GetAllActiveUserDto>>
    {
    }
}
