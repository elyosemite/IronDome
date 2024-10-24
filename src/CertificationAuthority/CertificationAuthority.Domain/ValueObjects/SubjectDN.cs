namespace CertificationAuthority.Domain.ValueObjects;

public struct SubjectDN : IEquatable<SubjectDN>
{
    public string Value { get; }

    public SubjectDN(string value)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentNullException("SubjectDN value can not be null ou empty.");

        Value = $"CN={value}";
    }

    public bool Equals(SubjectDN other)
    {
        return Value == other.Value;
    }
}