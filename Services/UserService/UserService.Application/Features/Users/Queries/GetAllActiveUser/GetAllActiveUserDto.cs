using System;
using System.Collections.Generic;
using System.Text;

namespace UserService.Application.Features.Users.Queries.GetAllActiveUser
{
    public record GetAllActiveUserDto(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        DateTime CreatedAt
    );
}
