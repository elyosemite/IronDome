namespace CertificationAuthority.Domain.Enumerations;

public class SignatureAlgorithmEnum(int id, string name) : Enumeration(id, name)
{
    public static readonly SignatureAlgorithmEnum Sha256WithRsa = new(1, nameof(Sha256WithRsa));
}
