using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Domain.Entities
{
    // ═══════════════════════════════════════════════════════════
    // TableauDeBord — config personnalisée par médecin
    // ═══════════════════════════════════════════════════════════
    public class TableauDeBord : BaseEntity
    {
        // ═══════════════════════════════════════════
        // IDENTITÉ
        // ═══════════════════════════════════════════
        public Guid CabinetId { get; private set; }
        public Guid ProprietaireId { get; private set; }   // MedecinId ou AdminId
        public string Nom { get; private set; } = default!;
        public bool EstParDefaut { get; private set; }

        // ═══════════════════════════════════════════
        // CONFIGURATION
        // ═══════════════════════════════════════════
        public TypeKPI[] KPIsAffiches { get; private set; } = [];
        public TypePeriode PeriodeDefaut { get; private set; }
        public int NbSemainesTendance { get; private set; }    // fenêtre graphique tendance

        // ═══════════════════════════════════════════
        // FACTORY
        // ═══════════════════════════════════════════
        public static TableauDeBord Create(
            Guid cabinetId,
            Guid proprietaireId,
            string nom,
            TypePeriode periodeDefaut = TypePeriode.Mensuel,
            bool estParDefaut = false)
        {
            return new TableauDeBord
            {
                CabinetId = cabinetId,
                ProprietaireId = proprietaireId,
                Nom = nom.Trim(),
                PeriodeDefaut = periodeDefaut,
                EstParDefaut = estParDefaut,
                NbSemainesTendance = 12,
                KPIsAffiches = [
                    TypeKPI.NbConsultations,
                TypeKPI.NbNouveauxPatients,
                TypeKPI.TauxOccupation,
                TypeKPI.ChiffreAffaires,
            ],
            };
        }

        // ═══════════════════════════════════════════
        // COMPORTEMENT
        // ═══════════════════════════════════════════
        public void AjouterKPI(TypeKPI kpi)
        {
            if (!KPIsAffiches.Contains(kpi))
            {
                KPIsAffiches = [.. KPIsAffiches, kpi];
                MarkUpdated();
            }
        }

        public void RetirerKPI(TypeKPI kpi)
        {
            KPIsAffiches = KPIsAffiches.Where(k => k != kpi).ToArray();
            MarkUpdated();
        }

        public void Personnaliser(
            string nom,
            TypeKPI[] kpisAffiches,
            TypePeriode periodeDefaut,
            int nbSemainesTendance)
        {
            if (string.IsNullOrWhiteSpace(nom))
                throw new ArgumentException("Le nom est obligatoire.");

            if (kpisAffiches.Length == 0)
                throw new ArgumentException("Au moins un KPI doit être affiché.");

            if (nbSemainesTendance is < 4 or > 52)
                throw new ArgumentOutOfRangeException(nameof(nbSemainesTendance), "Entre 4 et 52.");

            Nom = nom.Trim();
            KPIsAffiches = kpisAffiches;
            PeriodeDefaut = periodeDefaut;
            NbSemainesTendance = nbSemainesTendance;
            MarkUpdated();
        }

        public void DefinirCommeParDefaut()
        {
            EstParDefaut = true;
            MarkUpdated();
        }
    }
}
