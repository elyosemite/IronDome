using MediatR;

namespace CertificationAuthority.Application.UseCases.CreateCsrFile;

public record CreateCsrFileResponse(byte[] CsrFile, byte[] PrivateKey);
