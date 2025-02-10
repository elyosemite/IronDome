namespace CertificationAuthority.Domain.ValueObjects;

public struct PrivateKey : IEquatable<PrivateKey>
{
    public string Value { get; }

    public PrivateKey(string value)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value), "PrivateKey value can not be null or empty.");

        Value = value;
    }

    public bool Equals(PrivateKey other)
    {
        return Value == other.Value;
    }
}
