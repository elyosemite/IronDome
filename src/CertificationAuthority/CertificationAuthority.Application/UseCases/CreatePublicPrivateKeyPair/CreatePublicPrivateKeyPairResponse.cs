namespace CertificationAuthority.Application.UseCases.CreatePublicPrivateKeyPair;

public record CreatePublicPrivateKeyPairResponse(byte[] PublicKey, byte[] PrivateKey);