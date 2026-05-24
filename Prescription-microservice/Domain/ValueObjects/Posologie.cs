using Prescription_microservice.Domain.Enums;

namespace Prescription_microservice.Domain.ValueObjects
{
    public record Posologie(
        string Dose,
        string Unite,
        string Frequence,
        MomentPrise Moment
    )
    {
        public static Posologie Create(
            string dose,
            string unite,
            string frequence,
            MomentPrise moment)
        {
            if (string.IsNullOrWhiteSpace(dose))
                throw new ArgumentException("La dose est obligatoire.");
            if (string.IsNullOrWhiteSpace(unite))
                throw new ArgumentException("L'unité est obligatoire.");
            if (string.IsNullOrWhiteSpace(frequence))
                throw new ArgumentException("La fréquence est obligatoire.");

            return new Posologie(dose.Trim(), unite.Trim(), frequence.Trim(), moment);
        }

        // Rendu lisible : "500 mg — 3 fois/jour — Après le repas"
        public override string ToString()
            => $"{Dose} {Unite} — {Frequence} — {Moment.ToLabel()}";
    }
}
