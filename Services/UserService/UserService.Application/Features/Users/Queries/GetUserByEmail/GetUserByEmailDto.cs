using System;

namespace UserService.Application.Features.Users.Queries.GetUserByEmail;

public record GetUserByEmailDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    DateTime CreatedAt
);
