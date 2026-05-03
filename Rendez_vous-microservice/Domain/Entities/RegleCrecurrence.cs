using Rendez_vous_microservice.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Rendez_vous_microservice.Domain.Entities
{
    public class RegleRecurrence : BaseEntity
    {
        public Guid MedecinId { get; set; }
        public int JoursSemaineBits { get; set; }
        public TimeOnly HeureDebut { get; set; }
        public TimeOnly HeureFin { get; set; }
        public DateOnly DateDebut { get; set; }
        public DateOnly? DateFin { get; set; }

        public List<Creneau> GenererCreneaux(Guid cabinetId, TypeCreneau type)
        {
            var creneaux = new List<Creneau>();
            var dateFinEffective = DateFin ?? DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1));

            for (var date = DateDebut; date <= dateFinEffective; date = date.AddDays(1))
            {
                int dayBit = 1 << (int)date.DayOfWeek;
                if ((JoursSemaineBits & dayBit) != 0)
                {
                    var debut = date.ToDateTime(HeureDebut);
                    var fin = date.ToDateTime(HeureFin);
                    var creneau = Creneau.Create(MedecinId, cabinetId, debut, fin, type, this.Id);
                    creneaux.Add(creneau);
                }
            }
            return creneaux;
        }
    }
}
