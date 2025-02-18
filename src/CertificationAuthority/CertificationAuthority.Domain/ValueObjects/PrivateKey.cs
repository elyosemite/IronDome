namespace CertificationAuthority.Domain.ValueObjects;

public struct PrivateKey : IKey, IEquatable<PrivateKey>
{
    private readonly byte[] _key;

    public PrivateKey(byte[] key)
    {
        _key = key ?? throw new ArgumentNullException(nameof(key));
    }

    public PrivateKey(string base64Key)
    {
        if (string.IsNullOrEmpty(base64Key)) throw new ArgumentNullException(nameof(base64Key), "PrivateKey base64Key can not be null or empty.");

        _key = Convert.FromBase64String(base64Key);
    }

    public string Value
    {
        get => ToBase64();
    }

    public bool Equals(PrivateKey other)
    {
        return Value == other.Value;
    }

    public byte[] ToDer() => _key;
    public string ToPem() => $"-----BEGIN PRIVATE KEY-----\n{ToBase64()}\n-----END PRIVATE KEY-----";
    public string ToBase64() => Convert.ToBase64String(_key);
}
