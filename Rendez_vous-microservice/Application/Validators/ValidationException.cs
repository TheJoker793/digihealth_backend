namespace Rendez_vous_microservice.Application.Validators
{
    // Exception générique pour les validations
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }

    // Exception quand une ressource n'est pas trouvée
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    // Exception quand il y a un conflit métier (ex: doublon, état incompatible)
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
    // Exception spécifique aux créneaux indisponibles
    public class CreneauIndisponibleException : Exception
    {
        public CreneauIndisponibleException(string message) : base(message) { }
    }
}
