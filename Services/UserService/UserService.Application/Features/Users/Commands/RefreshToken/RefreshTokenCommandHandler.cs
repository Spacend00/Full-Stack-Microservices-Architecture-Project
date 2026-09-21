
using MediatR;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using UserService.Application.Common.Exceptions;
using UserService.Application.Features.Users.Commands.LoginUser;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Users.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginUserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        public RefreshTokenCommandHandler(IUserRepository userRepository, ITokenService tokenService, IConfiguration configuration, IDistributedCache cache)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _configuration = configuration;
            _cache = cache;
        }
        public async Task<LoginUserDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            User? user = null;
            var oldCacheKey = $"refreshToken:{request.RefreshToken}";

            var cachedUserId = await _cache.GetStringAsync(oldCacheKey, cancellationToken);

            if(!string.IsNullOrEmpty(cachedUserId) && Guid.TryParse(cachedUserId, out var userId))
            {
                user = await _userRepository.GetByIdAsync(userId);
            }
            else
            {
                user = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);
            }

            if(user is null || user.RefreshTokenExpireTime < DateTime.UtcNow)
            {
                throw new RefreshTokenException("Invalid or expired refresh token.");
            }

            await _cache.RemoveAsync(oldCacheKey, cancellationToken);

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenDays = double.TryParse(_configuration["JwtSettings:RefreshTokenExpiryDays"], out var days) ? days: 7;
            var refreshTokenExpireTime = DateTime.UtcNow.AddDays(refreshTokenDays);

            user.UpdateRefreshToken(newRefreshToken, refreshTokenExpireTime);
            await _userRepository.SaveChangesAsync();

            var newCacheKey = $"refreshToken:{newRefreshToken}";
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(refreshTokenDays)
            };
            await _cache.SetStringAsync(newCacheKey, user.Id.ToString(), cacheOptions, cancellationToken);

            return new LoginUserDto(newAccessToken, newRefreshToken, refreshTokenExpireTime);
        }
    }
}
