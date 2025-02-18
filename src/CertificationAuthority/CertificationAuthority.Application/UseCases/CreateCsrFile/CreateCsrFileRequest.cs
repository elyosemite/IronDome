using MediatR;

namespace CertificationAuthority.Application.UseCases.CreateCsrFile;

public record CreateCsrFileRequest(
    string SubjectDN,
    string PublicKey,
    string PrivateKey,
    string SignatureAlgorithm,
    string PublicKeyFilePath,
    string PrivateKeyFilePath) : IRequest<CreateCsrFileResponse>;
