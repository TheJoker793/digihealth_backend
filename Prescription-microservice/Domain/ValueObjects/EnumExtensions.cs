using Prescription_microservice.Domain.Enums;

namespace Prescription_microservice.Domain.ValueObjects
{
    public static class EnumExtensions
    {
        public static string ToLabel(this MomentPrise moment) => moment switch
        {
            MomentPrise.AvantRepas => "Avant le repas",
            MomentPrise.PendantRepas => "Pendant le repas",
            MomentPrise.ApresRepas => "Après le repas",
            MomentPrise.IndependantRepas => "Indépendamment des repas",
            MomentPrise.AuCoucher => "Au coucher",
            MomentPrise.AMatin => "Le matin",
            _ => moment.ToString()
        };
    }
}
