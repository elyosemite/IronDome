using CertificationAuthority.Application.Ports;

namespace CertificationAuthority.Application.UseCases.CreateFoo;

public class CreateFooHandler : IInputBoundary<CreateFooRequest, CreateFooResponse>
{
    public CreateFooHandler()
    {
    }

    public Task<CreateFooResponse> HandleAsync(CreateFooRequest request, CancellationToken cancellationToken)
    {
        //Create Foo and other processing...
        var response =  new CreateFooResponse();
        return Task.FromResult(response);
    }
}