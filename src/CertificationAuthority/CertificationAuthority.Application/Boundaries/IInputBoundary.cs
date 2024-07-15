namespace CertificationAuthority.Application.Ports;

public interface IInputBoundary<TRequest, TResponse>
{
    public Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}