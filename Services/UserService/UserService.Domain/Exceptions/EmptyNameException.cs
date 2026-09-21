using System;

namespace UserService.Domain.Exceptions;

public class EmptyNameException : DomainException
{
    public EmptyNameException(string propertyName) 
        : base($"{propertyName} cannot be null, empty, or whitespace.")
    {
    }
}
