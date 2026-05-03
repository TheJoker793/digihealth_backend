namespace Auth_microservice.Exceptions
{
    public class AccountLockedException : Exception
    {
        public AccountLockedException(string message = "Account is locked")
            : base(message)
        {
        }
    }
}
