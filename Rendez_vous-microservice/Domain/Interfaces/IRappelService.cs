namespace Rendez_vous_microservice.Domain.Interfaces
{
    public interface IRappelService
    {
        /// <summary>
        /// Programme les rappels pour un rendez-vous donné.
        /// </summary>
        Task ProgrammerRappelsAsync(Guid rendezVousId);

        /// <summary>
        /// Envoie les rappels J-1 (la veille du rendez-vous).
        /// </summary>
        Task EnvoyerRappelJ1();

        /// <summary>
        /// Envoie les rappels H-2 (2 heures avant le rendez-vous).
        /// </summary>
        Task EnvoyerRappelH2();
    }
}
