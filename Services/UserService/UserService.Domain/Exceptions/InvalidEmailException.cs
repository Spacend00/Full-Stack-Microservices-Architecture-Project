using System;

namespace UserService.Domain.Exceptions;

public class InvalidEmailException : DomainException
{
    public InvalidEmailException(string email) 
        : base($"The email address '{email}' is invalid.")
    {
    }
}
