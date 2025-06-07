using CertificationAuthority.Domain.Interfaces;

namespace CertificationAuthority.Infrastructure.Facades;

public class DerConvertFacade : IDerConvertFacade
{
    public byte[] ToDer(string derContent)
    {
        throw new NotImplementedException();
    }

    public byte[] ToDer(Stream derContent)
    {
        throw new NotImplementedException();
    }

    public byte[] ToDer(byte[] derContent)
    {
        throw new NotImplementedException();
    }

    public byte[] ToDerFromPem(string pemContent)
    {
        throw new NotImplementedException();
    }

    public byte[] ToDerFromPem(Stream pemContent)
    {
        throw new NotImplementedException();
    }

    public byte[] ToDerFromPem(byte[] pemContent)
    {
        throw new NotImplementedException();
    }
}