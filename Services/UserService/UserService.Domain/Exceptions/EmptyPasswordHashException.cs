using System;

namespace UserService.Domain.Exceptions;

public class EmptyPasswordHashException : DomainException
{
    public EmptyPasswordHashException() 
        : base("Password hash cannot be null, empty, or whitespace.")
    {
    }
}
