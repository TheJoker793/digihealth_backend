namespace Patient_microservice.Exceptions
{
    public class ValidationException : AppException
    {
        public ValidationException(string message) : base(message) { }
    }
}
