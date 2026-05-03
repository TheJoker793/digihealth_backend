namespace Patient_microservice.Domain.Enums
{
    public enum GroupeSanguin
    {
        APositif,
        ANegatif,
        BPositif,
        BNegatif,
        ABPositif,
        ABNegatif,
        OPositif,
        ONegatif
    }

    public static class GroupeSanguinExtensions
    {
        public static string ToDisplay(this GroupeSanguin groupe)
        {
            return groupe switch
            {
                GroupeSanguin.APositif => "A+",
                GroupeSanguin.ANegatif => "A-",
                GroupeSanguin.BPositif => "B+",
                GroupeSanguin.BNegatif => "B-",
                GroupeSanguin.ABPositif => "AB+",
                GroupeSanguin.ABNegatif => "AB-",
                GroupeSanguin.OPositif => "O+",
                GroupeSanguin.ONegatif => "O-",
                _ => throw new ArgumentOutOfRangeException(nameof(groupe))
            };
        }
    }
}
