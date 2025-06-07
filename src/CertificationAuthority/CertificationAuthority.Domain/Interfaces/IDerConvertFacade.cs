namespace CertificationAuthority.Domain.Interfaces;

public interface IDerConvertFacade
{
    byte[] ToDer(string derContent);
    byte[] ToDer(byte[] derContent);
    byte[] ToDer(Stream derContent);
    byte[] ToDerFromPem(string pemContent);
    byte[] ToDerFromPem(Stream pemContent);
    byte[] ToDerFromPem(byte[] pemContent);
}
