using MassTransit;
using Notification_microservice.Application.DTOs.Requests;
using Notification_microservice.Application.Services;
using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Application.Consumers
{
    public class OrdonnanceSigneeConsumer : IConsumer<OrdonnanceSigneeEvent>
    {
        private readonly NotificationService _service;
        public OrdonnanceSigneeConsumer(NotificationService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<OrdonnanceSigneeEvent> context)
        {
            var evt = context.Message;
            var variables = new Dictionary<string, string>
            {
                ["prescription_id"] = evt.PrescriptionId.ToString(),
            };

            await _service.EnvoyerAsync(new EnvoyerNotificationRequest(
                TypeEvenement.OrdonnanceSignee,
                evt.PatientId, "Patient",
                CanalEnvoi.Email,
                variables,
                SourceId: evt.PrescriptionId,
                ContactEmail: evt.PatientEmail,
                PieceJointeChemin: evt.CheminPdfMinIO),   // PDF ordonnance en PJ
                context.CancellationToken);
        }



    }
}
