using Document_microservice.Application.DTOs.Requests;
using Document_microservice.Application.DTOs.Responses;
using Document_microservice.Domain.Entities;
using Document_microservice.Domain.Enums;
using Document_microservice.Domain.Interfaces;

namespace Document_microservice.Application.Services
{
    public class TemplateService
    {
        private readonly IUnitOfWork _uow;

        public TemplateService(IUnitOfWork uow) => _uow = uow;

        public async Task<TemplateResponse> CreerAsync(
            CreerTemplateRequest request,
            CancellationToken ct = default)
        {
            var template = TemplateDocument.Create(
                request.Nom,
                request.TypeDocument,
                request.ContenuHtml,
                request.Variables,
                request.Version,
                request.Specialite,
                request.CabinetId,
                request.Description);

            await _uow.Templates.AddAsync(template, ct);
            await _uow.SaveChangesAsync(ct);

            return ToResponse(template);
        }

        public async Task<TemplateResponse> MettreAJourAsync(
            Guid templateId,
            MettreAJourTemplateRequest request,
            CancellationToken ct = default)
        {
            var template = await _uow.Templates.GetByIdAsync(templateId, ct)
                ?? throw new KeyNotFoundException($"Template {templateId} introuvable.");

            template.MettreAJour(request.ContenuHtml, request.Variables, request.NouvelleVersion);
            _uow.Templates.Update(template);
            await _uow.SaveChangesAsync(ct);

            return ToResponse(template);
        }

        public async Task<IEnumerable<TemplateResponse>> GetByTypeAsync(
            TypeDocument type,
            CancellationToken ct = default)
        {
            var templates = await _uow.Templates.GetByTypeAsync(type, ct);
            return templates.Select(ToResponse);
        }

        public async Task<TemplateResponse> GetByIdAsync(
            Guid templateId,
            CancellationToken ct = default)
        {
            var template = await _uow.Templates.GetByIdAsync(templateId, ct)
                ?? throw new KeyNotFoundException($"Template {templateId} introuvable.");

            return ToResponse(template);
        }

        public async Task DesactiverAsync(Guid templateId, CancellationToken ct = default)
        {
            var template = await _uow.Templates.GetByIdAsync(templateId, ct)
                ?? throw new KeyNotFoundException($"Template {templateId} introuvable.");

            template.Desactiver();
            _uow.Templates.Update(template);
            await _uow.SaveChangesAsync(ct);
        }

        private static TemplateResponse ToResponse(TemplateDocument t)
            => new(t.Id, t.Nom, t.Description, t.TypeDocument, t.Specialite,
                   t.Variables, t.Version, t.EstActif, t.CabinetId, t.CreatedAt);
    }
}
