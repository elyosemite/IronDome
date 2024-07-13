using CertificationAuthority.Domain.Certificate;
using CertificationAuthority.Domain.Enumerations;
using CertificationAuthority.Domain.Factories;

namespace CertificationAuthority.Domain.Builders;

public class CertificateBuilder : ICertificateBuilder
{
    private Guid _identifier;
    private string _issuerDN = string.Empty;
    private string _serialNumber = string.Empty;
    private DateTime _notBefore;
    private DateTime _notAfter;
    private string _subjectDN = string.Empty;
    private string _publicKey = string.Empty;
    private SignatureAlgorithmEnum _signatureAlgorithm = SignatureAlgorithmEnum.SHA256WithRSA;

    /// <summary>
    /// It will be useful when it is necessary to reassembly an instance from the database with identifier
    /// </summary>
    private bool _isInstanceReassembly = false;
    private readonly ICertificateFactory _certificateFactory;

    public CertificateBuilder(ICertificateFactory certificateFactory)
    {
        _certificateFactory = certificateFactory;
    }

    public ICertificateBuilder WithIdentifier(Guid identifier)
    {
        _identifier = identifier;
        _isInstanceReassembly = true;
        return this;
    }

    public ICertificateBuilder WithIssuerDN(string issuerDN)
    {
        _issuerDN = issuerDN;
        return this;
    }

    public ICertificateBuilder WithNotAfter(DateTime notAfter)
    {
        _notAfter = notAfter;
        return this;
    }

    public ICertificateBuilder WithNotBefore(DateTime notBefore)
    {
        _notBefore = notBefore;
        return this;
    }

    public ICertificateBuilder WithPublicKey(string publicKey)
    {
        _publicKey = publicKey;
        return this;
    }

    public ICertificateBuilder WithSerialNumber(string serialNumber)
    {
        _serialNumber = serialNumber;
        return this;
    }

    public ICertificateBuilder WithSignatureAlgorithm(SignatureAlgorithmEnum signatureAlgorithm)
    {
        _signatureAlgorithm = signatureAlgorithm;
        return this;
    }

    public ICertificateBuilder WithSubjectDN(string subjectDN)
    {
        _subjectDN = subjectDN;
        return this;
    }

    public PKICertificate Build()
    {
        if (_isInstanceReassembly)
        {
            return _certificateFactory.Factory(_identifier, _issuerDN, _serialNumber, _notBefore, _notAfter, _subjectDN, _publicKey, _signatureAlgorithm);
        }

        return _certificateFactory.Factory(_issuerDN, _serialNumber, _notBefore, _notAfter, _subjectDN, _publicKey, _signatureAlgorithm);
    }
}
