namespace CertificationAuthority.Infrastructure;

public interface IFileZip<TEntity>
{
    Task<byte[]> Zip(TEntity entity);
}
