namespace CertificationAuthority.Infrastructure;

public interface IPublicKeyPairGenerator
{
    PublicKeyPair Generate();
}
