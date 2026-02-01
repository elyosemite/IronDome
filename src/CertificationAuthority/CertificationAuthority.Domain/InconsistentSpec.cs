using Ardalis.Specification;
using CertificationAuthority.Domain.Certificate;

namespace CertificationAuthority.Domain;

public class InconsistentSpec : Specification<PkiCertificate>
{
    public override bool IsSatisfiedBy(PkiCertificate entity)
    {
        return entity.NotAfter > entity.NotBefore;
    }
}
