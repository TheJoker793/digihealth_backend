namespace Patient_microservice.Domain.Entities
{
    public class MedecinTraitant
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Specialite { get; set; }
        public string? Telephone { get; set; }
        public string? NumOrdre { get; set; }
        public string? Adresse { get; set; }
    }
}
