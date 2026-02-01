using CertificationAuthority.Domain.Certificate;
using CertificationAuthority.Domain.Enumerations;
using CertificationAuthority.Infrastructure.Extensions;
using MediatR;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.OpenSsl;
using System.Text;

namespace CertificationAuthority.Application.UseCases.CreateCsrFile;

public sealed class CreateCsrFileHandler : IRequestHandler<CreateCsrFileRequest, CreateCsrFileResponse>
{
    public CreateCsrFileHandler()
    {
    }

    public Task<CreateCsrFileResponse> Handle(CreateCsrFileRequest request, CancellationToken cancellationToken)
    {
        var csr = new Csr(request.SubjectDN, request.PublicKey, request.PrivateKey,
        SignatureAlgorithmEnum.Sha256WithRsa, request.PublicKeyFilePath, request.PrivateKeyFilePath);

        AsymmetricKeyParameter privateKey = KeyExtension.ParseFromPEMPrivateKey(csr.PrivateKey.Value);
        AsymmetricKeyParameter publicKey = KeyExtension.ParseFromPEMPublicKey(csr.PublicKey.Value);

        var subject = new X509Name(csr.SubjectDn.Value);
        var signatureFactory = new Asn1SignatureFactory("SHA256WithRSA", privateKey, new SecureRandom());
        var pkcs10 = new Pkcs10CertificationRequest(signatureFactory, subject, publicKey, null);

        byte[] csrBytes;
        byte[] privateKeyBytes;

        using (var sw = new StringWriter())
        using (var pemWriter = new PemWriter(sw))
        {
            pemWriter.WriteObject(pkcs10);
            csrBytes = Encoding.UTF8.GetBytes(sw.ToString());
        }

        using (var sw = new StringWriter())
        using (var pemWriter = new PemWriter(sw))
        {
            pemWriter.WriteObject(privateKey);
            privateKeyBytes = Encoding.UTF8.GetBytes(sw.ToString());
        }

        return Task.FromResult(new CreateCsrFileResponse(csrBytes, privateKeyBytes));
    }
}
