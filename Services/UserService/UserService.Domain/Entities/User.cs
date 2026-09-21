using System.Text.RegularExpressions;
using UserService.Domain.Exceptions;

namespace UserService.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpireTime { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private User() { }

    public User(Guid id, string firstName, string lastName, string email, string passwordHash)
    {
        Id = id != Guid.Empty ? id : throw new ArgumentException("Id cannot be empty.", nameof(id));
        
        ValidateFirstName(firstName);
        ValidateLastName(lastName);
        ValidateEmail(email);
        ValidatePasswordHash(passwordHash);

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive) throw new DeactiveOperationException("This user already inactive!");
        IsActive = false;
    }
    public void Activate()
    {
        if (IsActive) throw new DeactiveOperationException("This user already activated!");
        IsActive = true;
    }

    public void UpdateProfile(string? firstName, string? lastName)
    {
        bool IsUpdated = false;
        if(firstName != null)
        {
            ValidateFirstName(firstName);
            FirstName = firstName;
            IsUpdated = true;
        }
        if(lastName != null)
        {
            ValidateLastName(lastName);
            LastName = lastName;
            IsUpdated = true;
        }
        if (IsUpdated) TouchUpdate();
    }

    public void ChangePassword(string newPasswordHash)
    {
        ValidatePasswordHash(newPasswordHash);

        PasswordHash = newPasswordHash;
        TouchUpdate();
    }

    public void UpdateRefreshToken(string refreshToken, DateTime refreshTokenExpireTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpireTime = refreshTokenExpireTime;
        TouchUpdate();
    }

    public void RevokeRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpireTime = null;
        TouchUpdate();
    }

    private static void ValidateFirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new EmptyNameException(nameof(FirstName));
        }
    }

    private static void ValidateLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new EmptyNameException(nameof(LastName));
        }
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new InvalidEmailException(email);
        }
        var emailRegex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.Compiled);
        if (!emailRegex.IsMatch(email))
        {
            throw new InvalidEmailException(email);
        }
    }

    private static void ValidatePasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new EmptyPasswordHashException();
        }
    }

    private void TouchUpdate()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
