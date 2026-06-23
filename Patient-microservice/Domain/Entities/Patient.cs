using Patient_microservice.Domain.Enums;
using Patient_microservice.Domain.Events;
using System.Text.Json.Serialization;

namespace Patient_microservice.Domain.Entities
{
    public class Patient:BaseEntity
    {
        public required string  Nom { get; set; }
        public required string Prenom { get; set; }
        public required DateOnly DateNaissance { get; set; }
        public required Sexe Sexe { get; set; }
        public required string Nationalite { get; set; }
        public string? Profession { get; set; }
        public required GroupeSanguin GroupeSanguin { get; set; }
        public required StatutPatient StatutPatient { get; set; }
        public required string Telephone { get; set; }
        public string? Email { get; set; }
        public Guid? MedecinTraitantId { get; set; }


        public MedecinTraitant? MedecinTraitant { get; set; }

        [JsonIgnore]

        public ICollection<PieceIdentite> PieceIdentites { get; set; } = new List<PieceIdentite>();

        [JsonIgnore]

        public ICollection<CouvertureSociale> CouvertureSociales { get; set; } = new List<CouvertureSociale>();
        [JsonIgnore]
        public ICollection<ContactUrgence> ContactsUrgence { get; set; } = new List<ContactUrgence>();
        [JsonIgnore]
        public ICollection<AssuranceComplementaire> AssurancesComplementaires { get; set; } = new List<AssuranceComplementaire>();

        // Méthode métier pour fusionner avec un autre patient
        public void MergeWith(Guid targetPatientId)
        {
            // Ici tu peux ajouter la logique métier de fusion
            // (par exemple transférer des données, marquer comme doublon, etc.)

            // Déclenchement de l’événement de domaine
            AddDomainEvent(new PatientMergedEvent(this.Id, targetPatientId));
        }













    }
}
