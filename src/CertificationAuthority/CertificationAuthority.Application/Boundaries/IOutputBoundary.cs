namespace CertificationAuthority.Application.Ports;

public interface IOutputBoundary<TRequest, TResponse>
{
    public Task<TResponse> RenderAsync(TRequest request, CancellationToken cancellationToken = default);
}