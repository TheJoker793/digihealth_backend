using Statistique_microservice.Domain.Enums;
using Statistique_microservice.Domain.Events;
using Statistique_microservice.Domain.Exceptions;
using Statistique_microservice.Domain.ValueObjects;

namespace Statistique_microservice.Domain.Entities
{
    public class RapportStatistique : BaseEntity
    {
        // ═══════════════════════════════════════════
        // IDENTITÉ
        // ═══════════════════════════════════════════
        public string Numero { get; private set; } = default!;

        // ═══════════════════════════════════════════
        // TYPE ET PORTÉE
        // ═══════════════════════════════════════════
        public TypeRapport TypeRapport { get; private set; }
        public Guid CabinetId { get; private set; }
        public Guid? MedecinId { get; private set; }         // null = rapport cabinet complet

        // ═══════════════════════════════════════════
        // PÉRIODE ANALYSÉE (Value Object)
        // ═══════════════════════════════════════════
        public PeriodeAnalyse Periode { get; private set; } = default!;

        // ═══════════════════════════════════════════
        // STATUT
        // ═══════════════════════════════════════════
        public StatutRapport Statut { get; private set; }
        public string? MessageErreur { get; private set; }
        public DateTimeOffset? DateGeneration { get; private set; }
        public DateTimeOffset? DatePlanifiee { get; private set; }

        // ═══════════════════════════════════════════
        // DONNÉES CALCULÉES
        // ═══════════════════════════════════════════
        /// <summary>
        /// JSON sérialisé des données calculées (KPIs, tableaux, séries temporelles).
        /// Désérialisé par le service selon le TypeRapport.
        /// </summary>
        public string? DonneesJson { get; private set; }

        // ═══════════════════════════════════════════
        // FICHIERS EXPORTÉS
        // ═══════════════════════════════════════════
        public string? CheminFichierPdf { get; private set; }
        public string? CheminFichierExcel { get; private set; }

        // ═══════════════════════════════════════════
        // KPIs associés (composition)
        // ═══════════════════════════════════════════
        private readonly List<IndicateurKPI> _indicateurs = new();
        public IReadOnlyCollection<IndicateurKPI> Indicateurs => _indicateurs.AsReadOnly();

        // ═══════════════════════════════════════════
        // FACTORY
        // ═══════════════════════════════════════════
        public static RapportStatistique Create(
            string numero,
            TypeRapport typeRapport,
            Guid cabinetId,
            PeriodeAnalyse periode,
            Guid? medecinId = null,
            DateTimeOffset? datePlanifiee = null)
        {
            var rapport = new RapportStatistique
            {
                Numero = numero,
                TypeRapport = typeRapport,
                CabinetId = cabinetId,
                MedecinId = medecinId,
                Periode = periode,
                Statut = datePlanifiee.HasValue
                                    ? StatutRapport.Planifie
                                    : StatutRapport.EnAttente,
                DatePlanifiee = datePlanifiee,
            };

            return rapport;
        }

        // ═══════════════════════════════════════════
        // CYCLE DE VIE
        // ═══════════════════════════════════════════

        /// <summary>Marque le rapport en cours de génération.</summary>
        public void DemarrerGeneration()
        {
            if (Statut is StatutRapport.Annule)
                throw new RapportAnnuleException(Id);

            Statut = StatutRapport.EnCours;
            MarkUpdated();
        }

        /// <summary>Enregistre les données calculées et marque le rapport comme généré.</summary>
        public void MarquerGenere(string donneesJson)
        {
            if (Statut != StatutRapport.EnCours)
                throw new InvalidOperationException(
                    $"Impossible de marquer généré un rapport en statut {Statut}.");

            if (string.IsNullOrWhiteSpace(donneesJson))
                throw new ArgumentException("Les données JSON sont obligatoires.");

            DonneesJson = donneesJson;
            Statut = StatutRapport.Genere;
            DateGeneration = DateTimeOffset.UtcNow;
            MessageErreur = null;
            MarkUpdated();

            AddDomainEvent(new RapportGenereEvent(
                Id, CabinetId, TypeRapport, Periode));
        }

        /// <summary>Enregistre une erreur de génération.</summary>
        public void MarquerEchec(string messageErreur)
        {
            Statut = StatutRapport.Echoue;
            MessageErreur = messageErreur;
            MarkUpdated();
        }

        /// <summary>Enregistre le chemin du PDF exporté.</summary>
        public void EnregistrerExportPdf(string chemin)
        {
            if (Statut != StatutRapport.Genere)
                throw new InvalidOperationException("Le rapport doit être généré avant l'export.");

            CheminFichierPdf = chemin;
            MarkUpdated();
        }

        /// <summary>Enregistre le chemin du fichier Excel exporté.</summary>
        public void EnregistrerExportExcel(string chemin)
        {
            if (Statut != StatutRapport.Genere)
                throw new InvalidOperationException("Le rapport doit être généré avant l'export.");

            CheminFichierExcel = chemin;
            MarkUpdated();
        }

        /// <summary>Annule un rapport planifié ou en attente.</summary>
        public void Annuler(string motif)
        {
            if (Statut is StatutRapport.Genere or StatutRapport.EnCours)
                throw new InvalidOperationException(
                    "Impossible d'annuler un rapport déjà généré ou en cours.");

            Statut = StatutRapport.Annule;
            MessageErreur = $"Annulé : {motif}";
            MarkUpdated();
        }

        /// <summary>Ajoute un indicateur KPI calculé au rapport.</summary>
        public void AjouterIndicateur(IndicateurKPI indicateur)
        {
            if (indicateur.CabinetId != CabinetId)
                throw new ArgumentException("L'indicateur n'appartient pas au même cabinet.");

            _indicateurs.Add(indicateur);
            MarkUpdated();
        }

        // ═══════════════════════════════════════════
        // REQUÊTES MÉTIER
        // ═══════════════════════════════════════════
        public bool EstPret() => Statut == StatutRapport.Genere;
        public bool EstPlanifie() => Statut == StatutRapport.Planifie
                                       && DatePlanifiee <= DateTimeOffset.UtcNow;
        public bool AExportPdf() => !string.IsNullOrWhiteSpace(CheminFichierPdf);
        public bool AExportExcel() => !string.IsNullOrWhiteSpace(CheminFichierExcel);
    }
}
