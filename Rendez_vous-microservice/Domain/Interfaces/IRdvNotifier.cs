namespace Rendez_vous_microservice.Domain.Interfaces
{
    public interface IRdvNotifier
    {
        /// <summary>
        /// Notifie en temps réel la création d’un nouveau rendez-vous.
        /// </summary>
        Task NotifierNouveauRdvAsync(Guid medecinId, object rendezVous);

        /// <summary>
        /// Notifie en temps réel l’annulation d’un rendez-vous.
        /// </summary>
        Task NotifierAnnulationAsync(Guid medecinId, Guid rendezVousId);
    }
}
