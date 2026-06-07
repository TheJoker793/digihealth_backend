namespace Auth_microservice.Domain.Entities
{
    public class Cabinet:BaseEntity
    {
        public string Nom { get; set; }
        public string Email { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string? Telephone2 { get; set; }
        public bool Actif { get; set; }


    }
}
