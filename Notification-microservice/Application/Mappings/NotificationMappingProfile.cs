using AutoMapper;
using Notification_microservice.Application.DTOs.Responses;
using Notification_microservice.Domain.Entities;

namespace Notification_microservice.Application.Mappings
{
    public class NotificationMappingProfile:Profile
    {
        public NotificationMappingProfile()
        {
            // Notification → NotificationResponse
            CreateMap<Notification, NotificationResponse>()
                .ConstructUsing(src => new NotificationResponse(
                    src.Id,
                    src.Numero,
                    src.TypeEvenement,
                    src.DestinataireId,
                    src.TypeDestinataire,
                    src.Canal,
                    src.Sujet,
                    src.Statut,
                    src.NbTentatives,
                    src.MaxTentatives,
                    src.DateEnvoi,
                    src.DateProgrammee,
                    src.DerniereErreur,
                    src.CreatedAt,
                    null));

            // HistoriqueEnvoi → HistoriqueEnvoiResponse
            CreateMap<HistoriqueEnvoi, HistoriqueEnvoiResponse>()
                .ConstructUsing(src => new HistoriqueEnvoiResponse(
                    src.Id,
                    src.Canal,
                    src.Resultat,
                    src.MessageErreur,
                    src.ProviderResponse,
                    src.DureeMs,
                    src.DateTentative));

            // TemplateNotification → TemplateNotificationResponse
            CreateMap<TemplateNotification, TemplateNotificationResponse>()
                .ConstructUsing(src => new TemplateNotificationResponse(
                    src.Id,
                    src.Code,
                    src.TypeEvenement,
                    src.Canal,
                    src.Langue,
                    src.SujetTemplate,
                    src.Variables,
                    src.EstActif,
                    src.CreatedAt));

            // PreferenceNotification → PreferenceNotificationResponse
            CreateMap<PreferenceNotification, PreferenceNotificationResponse>()
                .ConstructUsing(src => new PreferenceNotificationResponse(
                    src.Id,
                    src.DestinataireId,
                    src.TypeDestinataire,
                    src.CanauxActifs,
                    src.HeureDebut.HasValue ? src.HeureDebut.Value.ToString("HH:mm") : null,
                    src.HeureFin.HasValue ? src.HeureFin.Value.ToString("HH:mm") : null,
                    src.EstOptOut,
                    src.Langue));
        }
    }
}
