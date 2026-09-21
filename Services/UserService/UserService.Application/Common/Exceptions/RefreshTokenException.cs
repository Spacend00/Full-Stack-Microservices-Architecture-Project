
namespace UserService.Application.Common.Exceptions
{
    public class RefreshTokenException : BaseApplicationException
    {
        public RefreshTokenException(string message) : base(message) { }
    }
}
