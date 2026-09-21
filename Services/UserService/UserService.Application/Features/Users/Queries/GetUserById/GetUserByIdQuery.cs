using System;
using MediatR;

namespace UserService.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<GetUserByIdDto?>;

