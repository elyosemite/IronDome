using CertificationAuthority.Domain.Enumerations;

namespace CertificationAuthority.Domain.ValueObjects;

public readonly struct SignatureAlgorithm : IEquatable<SignatureAlgorithm>
{
    public SignatureAlgorithmEnum Value { get; }

    public SignatureAlgorithm(SignatureAlgorithmEnum value)
    {
        Value = value;
    }

    public bool Equals(SignatureAlgorithm other)
    {
        return Value == other.Value;
    }
}
