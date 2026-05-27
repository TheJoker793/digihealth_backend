namespace Facturation_microservice.Domain.ValueObjects
{
    public record NumeroFacture(string Valeur)
    {
        public override string ToString()
        {
            return Valeur;
        }
    }
}