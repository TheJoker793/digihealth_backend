namespace Patient_microservice.Exceptions
{
    public abstract class AppException:Exception
    {
        protected AppException(string message) : base(message) { }
    }
}
