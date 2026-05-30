using MassTransit;
using Notification_microservice.Application.DTOs.Requests;
using Notification_microservice.Application.Services;
using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Application.Consumers
{
    public class RdvRappelConsumer : IConsumer<RdvRappelEvent>
    {
        private readonly NotificationService _service;
        public RdvRappelConsumer(NotificationService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<RdvConfirmeEvent> context)
        {
            var evt = context.Message;
            var variables = new Dictionary<string, string>
            {
                ["patient_nom"] = evt.PatientId.ToString(),
                ["medecin_nom"] = evt.MedecinNom,
                ["cabinet_nom"] = evt.CabinetNom,
                ["date_rdv"] = evt.DateRdv.ToString("dddd d MMMM yyyy à HH:mm"),
                ["motif"] = evt.MotifRdv,
            };

            // Email
            if (!string.IsNullOrWhiteSpace(evt.PatientEmail))
            {
                await _service.EnvoyerAsync(new EnvoyerNotificationRequest(
                    TypeEvenement: TypeEvenement.RdvConfirme,
                    DestinataireId: evt.PatientId,
                    TypeDestinataire: "Patient",
                    Canal: CanalEnvoi.Email,
                    Variables: variables,
                    SourceId: evt.RdvId,
                    ContactEmail: evt.PatientEmail),
                    context.CancellationToken);
            }

            // SMS
            if (!string.IsNullOrWhiteSpace(evt.PatientTelephone))
            {
                await _service.EnvoyerAsync(new EnvoyerNotificationRequest(
                    TypeEvenement: TypeEvenement.RdvConfirme,
                    DestinataireId: evt.PatientId,
                    TypeDestinataire: "Patient",
                    Canal: CanalEnvoi.SMS,
                    Variables: variables,
                    SourceId: evt.RdvId,
                    ContactTelephone: evt.PatientTelephone),
                    context.CancellationToken);
            }
        }
    }
}
