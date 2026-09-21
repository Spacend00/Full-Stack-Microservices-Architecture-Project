using MediatR;

namespace UserService.Application.Features.Users.Queries.GetUserByEmail;

public record GetUserByEmailQuery(string Email) : IRequest<GetUserByEmailDto?>;
