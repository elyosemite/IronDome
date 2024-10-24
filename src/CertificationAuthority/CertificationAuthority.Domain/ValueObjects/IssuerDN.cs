namespace CertificationAuthority.Domain.ValueObjects;

public struct IssuerDN : IEquatable<IssuerDN>
{
    public string Value { get; }

    public IssuerDN(string value)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentNullException("IssuerDN value can not be null ou empty.");

        Value = $"DN={value}";
    }

    public bool Equals(IssuerDN other)
    {
        return Value == other.Value;
    }
}
