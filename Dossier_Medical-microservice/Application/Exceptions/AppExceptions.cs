namespace Dossier_Medical_microservice.Application.Exceptions
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

        public class ValidationException : Exception
        {
            public ValidationException(string message) : base(message) { }
        }

        public class DossierDejaCloture : Exception
        {
            public DossierDejaCloture() : base("Le dossier médical est déjà clôturé.") { }
        }
        public class ConsultationDejaClotureException : Exception
        {
            public ConsultationDejaClotureException() : base("La consultation est déjà clôturée.") { }
        }

        public class ForbiddenException : Exception
        {
            public ForbiddenException(string message) : base(message) { }
        }

    }
}
