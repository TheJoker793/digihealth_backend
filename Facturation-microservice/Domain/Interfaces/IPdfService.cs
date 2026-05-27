using Facturation_microservice.Domain.Entities;

namespace Facturation_microservice.Domain.Interfaces
{
    public interface IPdfService
    {
        Task<byte[]> GenererFacturePdfAsync(Facture facture);

    }
}
