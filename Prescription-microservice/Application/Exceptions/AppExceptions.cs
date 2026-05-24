using System;

namespace Prescription_microservice.Application.Exceptions
{
    // Exception générique pour ressource introuvable
    public class NotFoundException(string message) : Exception(message)
    {
    }

    // Exception générique pour conflit métier (doublon, état incompatible)
    public class ConflictException(string message) : Exception(message)
    {
    }

    // Exception spécifique aux interactions bloquantes
    public class InteractionBloquanteException(string message) : Exception(message)
    {
    }

    // Exception quand une prescription est déjà validée
    public class PrescriptionDejaValideeException(string message) : Exception(message)
    {
    }
}
