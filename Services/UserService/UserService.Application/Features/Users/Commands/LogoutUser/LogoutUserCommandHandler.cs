
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Reflection.Metadata.Ecma335;
using UserService.Application.Interfaces;

namespace UserService.Application.Features.Users.Commands.LogoutUser
{
    public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDistributedCache _cache;
        public LogoutUserCommandHandler(IUserRepository userRepository, IDistributedCache cache)
        {
            _userRepository = userRepository;
            _cache = cache;
        }
        public async Task<bool> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            var refreshCacheKey = $"refreshToken:{request.RefreshToken}";
            await _cache.RemoveAsync(refreshCacheKey, cancellationToken);

            var user = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);
            if(user != null)
            {
                user.RevokeRefreshToken();
                await _userRepository.SaveChangesAsync();
            }

            var remaningTime = request.AccessTokenExpiration - DateTime.UtcNow;

            if(remaningTime > TimeSpan.Zero)
            {
                var blacklistCacheKey = $"blacklistedToken:{request.AccessToken}";
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = remaningTime
                };

                await _cache.SetStringAsync(blacklistCacheKey, "1", cacheOptions, cancellationToken);
            }

            return true;
        }
    }
}
