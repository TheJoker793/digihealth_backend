namespace Prescription_microservice.Domain.Enums
{
    public enum SeveriteInteraction
    {
        Information = 1,  // affichée — pas bloquante
        Avertissement = 2,  // alerte jaune — médecin doit confirmer
        ContreIndication = 3   // bloquante — justification obligatoire
    }
}
