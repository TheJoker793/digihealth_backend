namespace Facturation_microservice.Application.Exceptions
{
    public class AppExceptions
    {
        public class NotFoundException : Exception
        {
            public NotFoundException(string message) : base(message) { }
        }
        public class ConflictException : Exception
        {
            public ConflictException(string message) : base(message) { }
        }

        public class FactureDejaPayeeException : Exception { }

        public class MontantDepasseException : Exception { }

        public class FactureAnnuleeException : Exception { }
    }
}
