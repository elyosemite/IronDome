﻿using CertificationAuthority.Domain.Enumerations;
using MediatR;

namespace CertificationAuthority.Application.UseCases.CreateCertificate;

public record CreateCertificateRequest(
    string IssuerDN, 
    string SerialNumber, 
    DateTime NotBefore, 
    DateTime NotAfter, 
    string SubjectDN, 
    string PublicKey,
    string SenderPrivateKey,
    SignatureAlgorithmEnum SignatureAlgorithm) : IRequest<CreateCertificateResponse>;
