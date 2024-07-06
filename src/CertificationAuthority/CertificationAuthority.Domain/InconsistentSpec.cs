using Ardalis.Specification;
using CertificationAuthority.Domain.Certificate;

namespace CertificationAuthority.Domain;

public class InconsistentSpec : Specification<PKICertificate>
{
    public override bool IsSatisfiedBy(PKICertificate entity)
    {
        return entity.NotAfter > entity.NotBefore;
    }
}
