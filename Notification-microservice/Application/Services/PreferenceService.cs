using Notification_microservice.Application.DTOs.Requests;
using Notification_microservice.Application.DTOs.Responses;
using Notification_microservice.Domain.Entities;
using Notification_microservice.Domain.Enums;
using Notification_microservice.Domain.Interfaces;

namespace Notification_microservice.Application.Services
{
    public class PreferenceService
    {
        private readonly IUnitOfWork _uow;

        public PreferenceService(IUnitOfWork uow) => _uow = uow;

        public async Task<PreferenceNotificationResponse> GetOuCreerAsync(
            Guid destinataireId,
            string typeDestinataire,
            CancellationToken ct = default)
        {
            var pref = await _uow.Preferences
                .GetByDestinataireAsync(destinataireId, typeDestinataire, ct);

            if (pref is null)
            {
                // Créer les préférences par défaut
                pref = PreferenceNotification.Create(destinataireId, typeDestinataire);
                await _uow.Preferences.AddAsync(pref, ct);
                await _uow.SaveChangesAsync(ct);
            }

            return ToResponse(pref);
        }

        public async Task<PreferenceNotificationResponse> MettreAJourAsync(
            Guid destinataireId,
            string typeDestinataire,
            MettreAJourPreferenceRequest request,
            CancellationToken ct = default)
        {
            var pref = await _uow.Preferences
                .GetByDestinataireAsync(destinataireId, typeDestinataire, ct)
                ?? PreferenceNotification.Create(destinataireId, typeDestinataire);

            // Remettre tous les canaux à zéro et appliquer les nouveaux
            foreach (var canal in Enum.GetValues<CanalEnvoi>())
            {
                if (request.CanauxActifs.Contains(canal))
                    pref.ActiverCanal(canal);
                else
                    pref.DesactiverCanal(canal);
            }

            // Plage horaire
            if (request.HeureDebut != null && request.HeureFin != null)
            {
                pref.DefinirPlageHoraire(
                    TimeOnly.Parse(request.HeureDebut),
                    TimeOnly.Parse(request.HeureFin));
            }

            pref.ChangerLangue(request.Langue);

            _uow.Preferences.Update(pref);
            await _uow.SaveChangesAsync(ct);

            return ToResponse(pref);
        }

        public async Task OptOutAsync(
            Guid destinataireId,
            string typeDestinataire,
            CancellationToken ct = default)
        {
            var pref = await _uow.Preferences
                .GetByDestinataireAsync(destinataireId, typeDestinataire, ct)
                ?? PreferenceNotification.Create(destinataireId, typeDestinataire);

            pref.OptOutTotal();

            _uow.Preferences.Update(pref);
            await _uow.SaveChangesAsync(ct);
        }

        public async Task AnnulerOptOutAsync(
            Guid destinataireId,
            string typeDestinataire,
            CancellationToken ct = default)
        {
            var pref = await _uow.Preferences
                .GetByDestinataireAsync(destinataireId, typeDestinataire, ct)
                ?? throw new KeyNotFoundException(
                    $"Préférences introuvables pour {destinataireId}.");

            pref.AnnulerOptOut();
            _uow.Preferences.Update(pref);
            await _uow.SaveChangesAsync(ct);
        }

        private static PreferenceNotificationResponse ToResponse(PreferenceNotification p)
            => new(
                p.Id,
                p.DestinataireId,
                p.TypeDestinataire,
                p.CanauxActifs,
                p.HeureDebut?.ToString("HH:mm"),
                p.HeureFin?.ToString("HH:mm"),
                p.EstOptOut,
                p.Langue);
    }
}
