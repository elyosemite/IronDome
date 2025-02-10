namespace CertificationAuthority.Domain.ValueObjects;

public struct FilePath : IEquatable<FilePath>
{
    public string Value { get; }

    public FilePath(string value)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value), "FilePath value can not be null ou empty.");

        Value = value;
    }

    public bool Equals(FilePath other)
    {
        return Value == other.Value;
    }
}
