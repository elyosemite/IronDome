﻿using CertificationAuthority.Domain.Certificate;
using CertificationAuthority.Domain.Enumerations;

namespace CertificationAuthority.Domain.Builders;

public interface ICertificateBuilder
{
    ICertificateBuilder WithIdentifier(Guid identifier);
    ICertificateBuilder WithIssuerDN(string issuerDN);
    ICertificateBuilder WithSerialNumber(string serialNumber);
    ICertificateBuilder WithNotBefore(DateTime notBefore);
    ICertificateBuilder WithNotAfter(DateTime notAfter);
    ICertificateBuilder WithSubjectDN(string subjectDN);
    ICertificateBuilder WithPublicKey(string publicKey);
    ICertificateBuilder WithSignatureAlgorithm(SignatureAlgorithmEnum signatureAlgorithm);
    PKICertificate Build();
}
