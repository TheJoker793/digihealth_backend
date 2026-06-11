namespace Auth_microservice.Domain.Enums
{
    public enum Role
    {
        // 0 réservé = non défini, jamais assigné
        Unknown = 0,
        Admin = 1,
        Medecin = 2,
        Secretaire = 3,
        Infirmier = 4,
        Patient = 5
    }
}
