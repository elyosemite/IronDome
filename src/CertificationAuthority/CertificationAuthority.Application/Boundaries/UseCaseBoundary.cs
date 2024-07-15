using CertificationAuthority.Application.DataModels;

namespace CertificationAuthority.Application.Ports;

//#region UseCase (Application)

//class CreatePKICertificate : IInputBoundary<CreatePKICertificateRequest, CreatePKICertificateResponse>
//{
//    private readonly IOutputBoundary<PKICertificateResultRequest, PKICertificateResultResponse> _outputBoundary;

//    public CreatePKICertificate(IOutputBoundary<PKICertificateResultRequest, PKICertificateResultResponse> outputBoundary)
//    {
//        _outputBoundary = outputBoundary;
//    }

//    public async Task<CreatePKICertificateResponse> HandleAsync(
//        CreatePKICertificateRequest request,
//        CancellationToken cancellationToken = default)
//    {
//        // Processing data from request ...

//        // Inject instance via Dependency Injection

//        var resultRequest = new PKICertificateResultRequest();
//        PKICertificateResultResponse presenter = await _outputBoundary.ExecuteAsync(resultRequest);

//        var result = new byte[] { 1, 2, 3 };
//        return new CreatePKICertificateResponse(result);
//    }
//}
//#endregion

//#region Presenter
//class JSONPresenter : OutputBoundary
//    .WithRequest<int>
//    .WithResponse<string>
//{
//    public override Task<string> HandleAsync(int request, CancellationToken cancellationToken = default)
//        => Task.FromResult(string.Empty);
//}

//class XMLPresenter : OutputBoundary
//    .WithRequest<int>
//    .WithResponse<string>
//{
//    public override Task<string> HandleAsync(int request, CancellationToken cancellationToken = default)
//        => Task.FromResult(string.Empty);
//}

//class YAMLPresenter : OutputBoundary
//    .WithRequest<int>
//    .WithResponse<string>
//{
//    public override Task<string> HandleAsync(int request, CancellationToken cancellationToken = default)
//        => Task.FromResult(string.Empty);
//}
//#endregion