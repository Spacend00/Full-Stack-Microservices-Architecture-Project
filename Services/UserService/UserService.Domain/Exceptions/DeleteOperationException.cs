
namespace UserService.Domain.Exceptions
{
    public class DeactiveOperationException : DomainException
    {
        public DeactiveOperationException(string message) : base(message) { }
    }
}
