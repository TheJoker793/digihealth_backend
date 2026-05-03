using System;
using Rendez_vous_microservice.Domain.Enums;
using Rendez_vous_microservice.Domain.Events;

namespace Rendez_vous_microservice.Domain.Entities
{
    public class RendezVous : BaseEntity 
    {
        public Guid PatientId { get; set; }
        public Guid MedecinId { get; set; }
        public Guid CabinetId { get; set; }

        public DateTime DateHeure { get; set; }
        public int DureeMinutes { get; set; }

        public string Motif { get; set; } = string.Empty;
        public StatutRv Statut { get; set; }
        public TypeConsultation TypeConsultation { get; set; }

        public string? NoteSecretaire { get; set; }

        public bool RappelEnvoye { get; set; }
        public DateTime? RappelAt { get; set; }

        // Méthodes métier
        public void Planifier(Guid patientId, Guid medecinId, Guid cabinetId, DateTime dateHeure, int dureeMinutes, string motif, TypeConsultation type)
        {
            PatientId = patientId;
            MedecinId = medecinId;
            CabinetId = cabinetId;
            DateHeure = dateHeure;
            DureeMinutes = dureeMinutes;
            Motif = motif;
            TypeConsultation = type;
            Statut = StatutRv.EnAttente; // statut initial
            RappelEnvoye = false;
        }

        public void Confirmer()
        {
            Statut = StatutRv.Confirme;
            AddDomainEvent(new RdvConfirmeEvent(this.Id, this.PatientId));
        }


        public void Annuler(string raison)
        {
            Statut = StatutRv.Annule;
            NoteSecretaire = raison;
        }

        public void Reporter(DateTime nouvelleDate)
        {
            Statut = StatutRv.Reporte;
            DateHeure = nouvelleDate;
        }

        public void MarquerArrive()
        {
            Statut = StatutRv.Arrive;
        }

        public void DemarrerConsultation()
        {
            Statut = StatutRv.EnCours;
        }

        public void TerminerConsultation()
        {
            Statut = StatutRv.Termine;
        }

        public void EnvoyerRappel()
        {
            RappelEnvoye = true;
            RappelAt = DateTime.UtcNow;
        }
    }
}
