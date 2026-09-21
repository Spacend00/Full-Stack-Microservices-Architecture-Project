
namespace UserService.Application.Common.Exceptions
{
    public abstract class BaseApplicationException : Exception
    {
        protected BaseApplicationException(string message) : base(message) { }
        protected BaseApplicationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
