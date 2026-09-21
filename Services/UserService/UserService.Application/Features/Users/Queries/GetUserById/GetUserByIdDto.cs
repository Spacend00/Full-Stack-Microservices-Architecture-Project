using System;

namespace UserService.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    DateTime CreatedAt
);
