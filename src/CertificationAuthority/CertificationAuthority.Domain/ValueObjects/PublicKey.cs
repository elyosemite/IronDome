namespace CertificationAuthority.Domain.ValueObjects;

public struct PublicKey : IKey, IEquatable<PublicKey>
{
    private readonly byte[] _key;

    public PublicKey(byte[] key)
    {
        _key = key ?? throw new ArgumentNullException(nameof(key));
    }

    public PublicKey(string base64)
    {
        _key = Convert.FromBase64String(base64);
    }

    public string Value
    {
        get => ToBase64();
    }

    public bool Equals(PublicKey other)
    {
        return Value == other.Value;
    }

    public byte[] ToDer() => _key;
    public string ToPem() => $"-----BEGIN PUBLIC KEY-----\n{ToBase64()}\n-----END PUBLIC KEY-----";
    public string ToBase64() => Convert.ToBase64String(_key);
}
