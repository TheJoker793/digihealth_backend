namespace Document_microservice.Domain.Exceptions
{
    public class DocumentNotFoundException(Guid id)
    : Exception($"Document {id} introuvable.");

    public class DocumentArchiveException(Guid id)
        : Exception($"Le document {id} est archivé — opération interdite.");

    public class VersionIntrouvableException(Guid documentId, int numero)
        : Exception($"Version {numero} introuvable pour le document {documentId}.");

    public class PartageExpireException(string token)
        : Exception($"Le partage avec le token {token} est expiré ou révoqué.");

    public class FichierTropGrandException(long tailleOctets, long limiteOctets)
        : Exception($"Fichier trop grand : {tailleOctets} octets (limite : {limiteOctets}).");

    public class FormatNonAutoriseException(string format)
        : Exception($"Format de fichier non autorisé : {format}.");

    public class VirusDetecteException(string nomVirus)
        : Exception($"Virus détecté dans le fichier : {nomVirus}. Upload refusé.");
}
