using Document_microservice.Domain.Entities;
using Document_microservice.Domain.Enums;
using Document_microservice.Domain.Interfaces;
using Document_microservice.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Document_microservice.Infrastructure.Repositories
{
    public class DocumentRepository
        : Repository<DocumentMedical>,
          IDocumentRepository
    {
        public DocumentRepository(DocumentDbContext context)
            : base(context)
        {
        }

        public async Task<DocumentMedical?> GetAvecVersionsAsync(
            Guid id,
            CancellationToken ct = default)
        {
            return await _dbSet
                .Include(d => d.Versions)
                .FirstOrDefaultAsync(d => d.Id == id, ct);
        }

        public async Task<DocumentMedical?> GetAvecPartagesAsync(
            Guid id,
            CancellationToken ct = default)
        {
            return await _dbSet
                .Include(d => d.Partages)
                .FirstOrDefaultAsync(d => d.Id == id, ct);
        }

        public async Task<IEnumerable<DocumentMedical>> GetByPatientAsync(
            Guid patientId,
            CancellationToken ct = default)
        {
            return await _dbSet
                .Where(d => d.PatientId == patientId)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<DocumentMedical>> GetByCabinetAsync(
            Guid cabinetId,
            TypeDocument? type = null,
            StatutDocument? statut = null,
            CancellationToken ct = default)
        {
            IQueryable<DocumentMedical> query = _dbSet
                .Where(d => d.CabinetId == cabinetId);

            if (type.HasValue)
                query = query.Where(d => d.TypeDocument == type.Value);

            if (statut.HasValue)
                query = query.Where(d => d.Statut == statut.Value);

            return await query
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<DocumentMedical>> GetByConsultationAsync(
            Guid consultationId,
            CancellationToken ct = default)
        {
            return await _dbSet
                .Where(d => d.ConsultationId == consultationId)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<bool> NumeroExisteAsync(
            string numero,
            CancellationToken ct = default)
        {
            return await _dbSet
                .AnyAsync(d => d.Numero == numero, ct);
        }
    }
}