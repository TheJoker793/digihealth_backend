namespace Auth_microservice.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message = "Conflict occurred")
            : base(message)
        {
        }
    }
}
