namespace CertificationAuthority.Domain.ValueObjects;

public struct PublicKey : IEquatable<PublicKey>
{
    public string Value { get; }

    public PublicKey(string value)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentNullException("PublicKey value can not be null ou empty.");

        Value = value;
    }

    public bool Equals(PublicKey other)
    {
        return Value == other.Value;
    }
}
