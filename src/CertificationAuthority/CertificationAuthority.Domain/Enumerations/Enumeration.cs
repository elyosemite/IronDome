namespace CertificationAuthority.Domain.Enumerations;

public abstract class Enumeration : IComparable<Enumeration>, IEquatable<Enumeration>
{
    public string Name { get; private set; }
    public int Id { get; private set; }

    protected Enumeration(int id, string name) => (Id, Name) = (id, name);

    public override string ToString() => Name;

    public bool Equals(Enumeration? other)
    {
        if (other is null) return false;

        return Id.Equals(other.Id);
    }

    public int CompareTo(Enumeration? other)
    {
        if (other is null) return 1;
        return Id.CompareTo((other).Id);
    } 
}
