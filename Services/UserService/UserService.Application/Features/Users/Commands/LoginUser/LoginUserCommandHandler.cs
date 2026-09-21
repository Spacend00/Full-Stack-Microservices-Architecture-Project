
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using UserService.Application.Common.Exceptions;
using UserService.Application.Interfaces;

namespace UserService.Application.Features.Users.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        public LoginUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService, IConfiguration configuration, IDistributedCache cache)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _configuration = configuration;
            _cache = cache;
        }

        public async Task<LoginUserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user is null) throw new UnauthorizedAccessException("Invalid Email or password.");

            var isVerify = _passwordHasher.Verify(request.Password, user.PasswordHash);
            if (!isVerify) throw new UnauthorizedAccessException("Invalid Email or password.");

            if (!user.IsActive) throw new UserInactiveException();

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenDays = double.TryParse(_configuration["JwtSettings:RefreshTokenExpiryDays"], out var days) ? days: 7;
            var refreshTokenExpireTime = DateTime.UtcNow.AddDays(refreshTokenDays);

            user.UpdateRefreshToken(refreshToken, refreshTokenExpireTime);
            await _userRepository.SaveChangesAsync();

            var cacheKey = $"refreshToken:{refreshToken}";
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(refreshTokenDays)
            };

            await _cache.SetStringAsync(cacheKey, user.Id.ToString(), cacheOptions, cancellationToken);

            return new LoginUserDto(accessToken, refreshToken, refreshTokenExpireTime);
        }
    }
}
