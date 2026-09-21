using System;

namespace UserService.Application.Features.Users.Queries.GetAllUser;

public record GetAllUserDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    bool isdeleted,
    DateTime CreatedAt
);
