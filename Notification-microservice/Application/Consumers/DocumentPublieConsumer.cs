using MassTransit;
using Notification_microservice.Application.DTOs.Requests;
using Notification_microservice.Application.Services;
using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Application.Consumers
{
    public class DocumentPublieConsumer : IConsumer<DocumentPublieEvent>
    {
        private readonly NotificationService _service;
        public DocumentPublieConsumer(NotificationService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<DocumentPublieEvent> context)
        {
            var evt = context.Message;
            var variables = new Dictionary<string, string>
            {
                ["type_document"] = evt.TypeDocument,
                ["titre"] = evt.TitreDocument,
            };

            if (!string.IsNullOrWhiteSpace(evt.PatientEmail))
                await _service.EnvoyerAsync(new EnvoyerNotificationRequest(
                    TypeEvenement.DocumentPublie, evt.PatientId, "Patient", CanalEnvoi.Email,
                    variables, SourceId: evt.DocumentId, ContactEmail: evt.PatientEmail),
                    context.CancellationToken);

            if (!string.IsNullOrWhiteSpace(evt.TokenFcm))
                await _service.EnvoyerAsync(new EnvoyerNotificationRequest(
                    TypeEvenement.DocumentPublie, evt.PatientId, "Patient", CanalEnvoi.Push,
                    variables, SourceId: evt.DocumentId, TokenFcm: evt.TokenFcm),
                    context.CancellationToken);
        }

    }
}
