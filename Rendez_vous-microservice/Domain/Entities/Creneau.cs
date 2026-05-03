using System;
using Rendez_vous_microservice.Domain.Enums;

namespace Rendez_vous_microservice.Domain.Entities
{
    public class Creneau : BaseEntity
    {
        public Guid MedecinId { get; set; }
        public Guid CabinetId { get; set; }
        public DateTime Debut { get; set; }
        public DateTime Fin { get; set; }
        public bool EstDisponible { get; private set; }
        public TypeCreneau TypeCreneau { get; set; }
        public Guid? RecurrenceId { get; set; }

        private Creneau() { }

        public static Creneau Create(Guid medecinId, Guid cabinetId, DateTime debut, DateTime fin, TypeCreneau type, Guid? recurrenceId = null)
        {
            if (fin <= debut)
                throw new ArgumentException("La fin doit être après le début.");
            return new Creneau
            {
                MedecinId = medecinId,
                CabinetId = cabinetId,
                Debut = debut,
                Fin = fin,
                EstDisponible = true,
                TypeCreneau = type,
                RecurrenceId = recurrenceId
            };
        }

        public void Bloquer() => EstDisponible = false;
        public void Liberer() => EstDisponible = true;
        public bool Chevauche(Creneau autre) => Debut < autre.Fin && autre.Debut < Fin;
    }
}
