
namespace UserService.Application.Common.Exceptions
{
    public class UserInactiveException : BaseApplicationException
    {
        public UserInactiveException() : base("User account is passive.") { }
    }
}
