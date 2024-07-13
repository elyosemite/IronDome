namespace CertificationAuthority.Domain.ValueObjects;

public struct TemporalPeriod : IEquatable<TemporalPeriod>
{
    public DateTime NotBefore { get; }
    public DateTime NotAfter { get; }

    public TemporalPeriod(DateTime norBefode, DateTime notAfter)
    {
        NotBefore = norBefode;
        NotAfter = notAfter;
    }

    public bool Equals(TemporalPeriod other)
    {
        return NotBefore.Equals(other.NotBefore) && NotAfter.Equals(other.NotAfter);
    }
}
