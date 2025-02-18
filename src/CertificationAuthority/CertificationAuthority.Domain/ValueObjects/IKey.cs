namespace CertificationAuthority.Domain.ValueObjects;

public interface IKey
{
    byte[] ToDer();
    string ToPem();
    string ToBase64();
}
