using MassTransit;
using Notification_microservice.Application.DTOs.Requests;
using Notification_microservice.Application.Services;
using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Application.Consumers
{
    public class RdvAnnuleConsumer : IConsumer<RdvAnnuleEvent>
    {
        private readonly NotificationService _service;
        public RdvAnnuleConsumer(NotificationService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<RdvAnnuleEvent> context)
        {
            var evt = context.Message;
            var variables = new Dictionary<string, string>
            {
                ["date_rdv"] = evt.DateRdv.ToString("dddd d MMMM yyyy à HH:mm"),
                ["motif_annulation"] = evt.MotifAnnulation,
            };

            if (!string.IsNullOrWhiteSpace(evt.PatientEmail))
                await _service.EnvoyerAsync(new EnvoyerNotificationRequest(
                    TypeEvenement.RdvAnnule, evt.PatientId, "Patient", CanalEnvoi.Email,
                    variables, SourceId: evt.RdvId, ContactEmail: evt.PatientEmail),
                    context.CancellationToken);

            if (!string.IsNullOrWhiteSpace(evt.PatientTelephone))
                await _service.EnvoyerAsync(new EnvoyerNotificationRequest(
                    TypeEvenement.RdvAnnule, evt.PatientId, "Patient", CanalEnvoi.SMS,
                    variables, SourceId: evt.RdvId, ContactTelephone: evt.PatientTelephone),
                    context.CancellationToken);
        }

    }
}
