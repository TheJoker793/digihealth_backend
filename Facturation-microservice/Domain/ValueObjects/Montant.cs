namespace Facturation_microservice.Domain.ValueObjects
{
    public record Montant(decimal Valeur, string Devise)
    {
        public override string ToString()
        {
            return $"{Valeur:N3} {Devise}";
        }
    }
}