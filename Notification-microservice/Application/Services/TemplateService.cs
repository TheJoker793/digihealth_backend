using Notification_microservice.Application.DTOs.Requests;
using Notification_microservice.Application.DTOs.Responses;
using Notification_microservice.Domain.Entities;
using Notification_microservice.Domain.Enums;
using Notification_microservice.Domain.Interfaces;
using static Notification_microservice.Domain.Exceptions.NotificationExceptions;

namespace Notification_microservice.Application.Services
{
    public class TemplateService
    {
        private readonly IUnitOfWork _uow;
        public TemplateService(IUnitOfWork unit)
        {
            _uow = unit;
        }
        public async Task<TemplateNotificationResponse> CreerAsync(
        CreerTemplateRequest request,
        CancellationToken ct = default)
        {
            // Vérifier que le code n'existe pas déjà
            var existant = await _uow.Templates.GetByCodeAsync(request.Code, ct);
            if (existant is not null)
                throw new InvalidOperationException(
                    $"Un template avec le code '{request.Code}' existe déjà.");

            var template = TemplateNotification.Create(
                request.Code,
                request.TypeEvenement,
                request.Canal,
                request.Langue,
                request.SujetTemplate,
                request.CorpsTemplate,
                request.Variables);

            await _uow.Templates.AddAsync(template, ct);
            await _uow.SaveChangesAsync(ct);

            return ToResponse(template);
        }

        public async Task<TemplateNotificationResponse> MettreAJourAsync(
            Guid id,
            MettreAJourTemplateRequest request,
            CancellationToken ct = default)
        {
            var template = await _uow.Templates.GetByIdAsync(id, ct)
                ?? throw new TemplateIntrouvableException(id.ToString());

            template.MettreAJour(
                request.SujetTemplate,
                request.CorpsTemplate,
                request.Variables);

            _uow.Templates.Update(template);
            await _uow.SaveChangesAsync(ct);

            return ToResponse(template);
        }

        public async Task<TemplateNotificationResponse> GetByIdAsync(
            Guid id, CancellationToken ct = default)
        {
            var template = await _uow.Templates.GetByIdAsync(id, ct)
                ?? throw new TemplateIntrouvableException(id.ToString());

            return ToResponse(template);
        }

        public async Task<IEnumerable<TemplateNotificationResponse>> GetByEvenementAsync(
            TypeEvenement typeEvenement, CancellationToken ct = default)
        {
            var templates = await _uow.Templates.GetByEvenementAsync(typeEvenement, ct);
            return templates.Select(ToResponse);
        }

        public async Task DesactiverAsync(Guid id, CancellationToken ct = default)
        {
            var template = await _uow.Templates.GetByIdAsync(id, ct)
                ?? throw new TemplateIntrouvableException(id.ToString());

            template.Desactiver();
            _uow.Templates.Update(template);
            await _uow.SaveChangesAsync(ct);
        }

        public async Task ActiverAsync(Guid id, CancellationToken ct = default)
        {
            var template = await _uow.Templates.GetByIdAsync(id, ct)
                ?? throw new TemplateIntrouvableException(id.ToString());

            template.Activer();
            _uow.Templates.Update(template);
            await _uow.SaveChangesAsync(ct);
        }

        private static TemplateNotificationResponse ToResponse(TemplateNotification t)
            => new(t.Id, t.Code, t.TypeEvenement, t.Canal,
                   t.Langue, t.SujetTemplate, t.Variables,
                   t.EstActif, t.CreatedAt);
    }


}

