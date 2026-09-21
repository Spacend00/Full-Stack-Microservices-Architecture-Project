using MediatR;

namespace UserService.Application.Features.Users.Queries.GetAllUser;

public record GetAllUserQuery : IRequest<IReadOnlyList<GetAllUserDto>>;
