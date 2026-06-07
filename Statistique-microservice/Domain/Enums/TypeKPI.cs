namespace Statistique_microservice.Domain.Enums
{
    // ═══════════════════════════════════════════════════════════
    // TypeKPI — indicateur calculable
    // ═══════════════════════════════════════════════════════════
    public enum TypeKPI
    {
        // ── Activité ──────────────────────────────────────────
        NbConsultations = 1,
        NbNouveauxPatients = 2,
        NbPatientsUniques = 3,
        TauxOccupation = 4,   // %
        NbRdvConfirmes = 5,
        TauxAnnulationRdv = 6,   // %
        NbOrdonnances = 7,

        // ── Finance ───────────────────────────────────────────
        ChiffreAffaires = 20,
        TicketMoyenConsultation = 21,  // TND
        TauxRecouvrement = 22,  // %
        MontantImpaye = 23,

        // ── Qualité ───────────────────────────────────────────
        DelaiMoyenConsultation = 30,  // minutes
        TauxRetourPatient = 31,  // % patients récurrents
    }
}
