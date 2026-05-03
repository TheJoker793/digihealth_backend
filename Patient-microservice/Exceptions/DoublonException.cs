namespace Patient_microservice.Exceptions
{
    public class DoublonException : AppException
    {
        public DoublonException(string message) : base(message) { }
    }
}
