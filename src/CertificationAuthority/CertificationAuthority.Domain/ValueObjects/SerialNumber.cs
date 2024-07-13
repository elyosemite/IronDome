namespace CertificationAuthority.Domain.ValueObjects;

public struct SerialNumber : IEquatable<SerialNumber>
{
    public string Value { get; }

    public SerialNumber(string value)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentNullException("SerialNumber value can not be null ou empty.");

        Value = value;
    }

    public bool Equals(SerialNumber other)
    {
        return Value == other.Value;
    }
}
