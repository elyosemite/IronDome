namespace CertificationAuthority.Domain.Enumerations;

public class SignatureAlgorithmEnum : Enumeration
{
    public static SignatureAlgorithmEnum SHA256WithRSA = new(1, nameof(SHA256WithRSA));

    public SignatureAlgorithmEnum(int id, string name) : base(id, name)
    {
    }
}
