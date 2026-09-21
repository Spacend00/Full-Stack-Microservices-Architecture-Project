using System;
using UserService.Domain.Entities;

namespace UserService.Application.Interfaces;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hashedPassword);
}
