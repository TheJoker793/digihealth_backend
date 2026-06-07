namespace Auth_microservice.DTOs.Responses
{
    public class CabinetDto
    {
        public Guid Id { get; set; }
        public string Nom { get; set; }
        public string Email { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string Telephone2 { get; set; }
        public bool Actif { get; set; }
        public DateOnly DateCreation { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
