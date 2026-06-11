using Statistique_microservice.Domain.Events;

namespace Statistique_microservice.Domain.Entities
{
    /// <summary>
    /// Photo quotidienne de l'activité d'un cabinet.
    /// Construite de façon incrémentale via les events RabbitMQ.
    /// Consolidée définitivement à 23h55 chaque jour.
    /// </summary>
    public class SnapshotActivite : BaseEntity
    {
        // ═══════════════════════════════════════════
        // IDENTITÉ
        // ═══════════════════════════════════════════
        public Guid CabinetId { get; private set; }
        public DateOnly DateSnapshot { get; private set; }
        public bool EstConsolide { get; private set; }      // true = définitif, plus modifiable

        // ═══════════════════════════════════════════
        // ACTIVITÉ CONSULTATIONS
        // ═══════════════════════════════════════════
        public int NbConsultations { get; private set; }
        public int NbConsultationsAnnulees { get; private set; }
        public int NbNouveauxPatients { get; private set; }
        public int NbPatientsUniques { get; private set; }  // distincts sur la journée

        // ═══════════════════════════════════════════
        // ACTIVITÉ RDV
        // ═══════════════════════════════════════════
        public int NbRdvConfirmes { get; private set; }
        public int NbRdvAnnules { get; private set; }
        public int NbRdvHonnores { get; private set; }

        // ═══════════════════════════════════════════
        // ACTIVITÉ PRESCRIPTIONS / DOCUMENTS
        // ═══════════════════════════════════════════
        public int NbOrdonnances { get; private set; }
        public int NbDocumentsGeneres { get; private set; }

        // ═══════════════════════════════════════════
        // ACTIVITÉ FINANCIÈRE
        // ═══════════════════════════════════════════
        public decimal ChiffreAffaires { get; private set; }    // somme factures du jour
        public int NbFactures { get; private set; }
        public decimal MontantImpaye { get; private set; }

        // ═══════════════════════════════════════════
        // TAUX CALCULÉS
        // ═══════════════════════════════════════════
        public decimal TauxOccupation { get; private set; }     // NbConsultations / créneau dispo

        // ═══════════════════════════════════════════
        // FACTORY
        // ═══════════════════════════════════════════
        public static SnapshotActivite Creer(Guid cabinetId, DateOnly date)
        {
            return new SnapshotActivite
            {
                CabinetId = cabinetId,
                DateSnapshot = date,
                EstConsolide = false,
            };
        }

        // ═══════════════════════════════════════════
        // INCRÉMENTS (appelés par les consumers)
        // ═══════════════════════════════════════════
        public void AjouterConsultation(bool estAnnulee = false)
        {
            VerifierNonConsolide();
            if (estAnnulee) NbConsultationsAnnulees++;
            else NbConsultations++;
            MarkUpdated();
        }

        public void AjouterNouveauPatient()
        {
            VerifierNonConsolide();
            NbNouveauxPatients++;
            NbPatientsUniques++;
            MarkUpdated();
        }

        public void AjouterPatientExistant()
        {
            VerifierNonConsolide();
            NbPatientsUniques++;
            MarkUpdated();
        }

        public void AjouterRdv(bool confirme, bool annule = false, bool honore = false)
        {
            VerifierNonConsolide();
            if (confirme) NbRdvConfirmes++;
            if (annule) NbRdvAnnules++;
            if (honore) NbRdvHonnores++;
            MarkUpdated();
        }

        public void AjouterOrdonnance()
        {
            VerifierNonConsolide();
            NbOrdonnances++;
            MarkUpdated();
        }

        public void AjouterDocument()
        {
            VerifierNonConsolide();
            NbDocumentsGeneres++;
            MarkUpdated();
        }

        public void AjouterFacture(decimal montant, bool estImpayee = false)
        {
            VerifierNonConsolide();
            NbFactures++;
            ChiffreAffaires += montant;
            if (estImpayee) MontantImpaye += montant;
            MarkUpdated();
        }

        // ═══════════════════════════════════════════
        // CONSOLIDATION (fin de journée à 23h55)
        // ═══════════════════════════════════════════

        /// <summary>
        /// Calcule les taux dérivés et verrouille le snapshot.
        /// Appelé par ConsolidationSnapshotJob.
        /// </summary>
        public void Consolider(int creneauxDisponibles)
        {
            VerifierNonConsolide();

            if (creneauxDisponibles > 0)
                TauxOccupation = Math.Round(
                    (decimal)NbConsultations / creneauxDisponibles * 100m, 2);

            EstConsolide = true;
            MarkUpdated();

            AddDomainEvent(new SnapshotConsolideEvent(Id, CabinetId, DateSnapshot));
        }

        // ═══════════════════════════════════════════
        // REQUÊTES MÉTIER
        // ═══════════════════════════════════════════
        public bool EstComplet()
            => EstConsolide && DateSnapshot < DateOnly.FromDateTime(DateTime.UtcNow);

        public decimal TicketMoyen()
            => NbFactures > 0 ? Math.Round(ChiffreAffaires / NbFactures, 3) : 0m;

        public decimal TauxAnnulationRdv()
            => NbRdvConfirmes > 0
                ? Math.Round((decimal)NbRdvAnnules / NbRdvConfirmes * 100m, 2)
                : 0m;

        // ═══════════════════════════════════════════
        // HELPER PRIVÉ
        // ═══════════════════════════════════════════
        private void VerifierNonConsolide()
        {
            if (EstConsolide)
                throw new InvalidOperationException(
                    $"Le snapshot du {DateSnapshot} est consolidé — modification interdite.");
        }
    }
}
