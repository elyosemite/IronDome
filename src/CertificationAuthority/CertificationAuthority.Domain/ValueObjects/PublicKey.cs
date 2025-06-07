using CertificationAuthority.Domain.Extensions;
using CertificationAuthority.Domain.Interfaces;

namespace CertificationAuthority.Domain.ValueObjects;

public enum KeyEncodingType
{
    Der,
    Pem,
    Unknown
}

public struct PublicKey : IEquatable<PublicKey>
{
    private readonly byte[] _key;
    private readonly KeyEncodingType _keyEncodingType = KeyEncodingType.Unknown;

    public PublicKey(byte[] key)
    {
        _key = key ?? throw new ArgumentNullException(nameof(key));
        _keyEncodingType = KeyEncodingType.Der;
    }

    // FIXME: This constructor should not exists. Remove in future versions.
    public PublicKey(string base64)
    {
        _key = Convert.FromBase64String(base64);
        _keyEncodingType = KeyEncodingType.Pem;
    }

    public string Value
    {
        get => this.ToBase64();
    }

    public byte[] RawValue
    {
        get
        {
            if (_key == null || _key.Length == 0)
                throw new InvalidOperationException("Public key is not initialized.");

            return _key;
        }
    }

    public KeyEncodingType KeyEncodingType => _keyEncodingType;

    public bool Equals(PublicKey other)
    {
        return Value == other.Value;
    }

    public static PublicKey FromDer(string der, IDerConvertFacade derConvertFacade)
    {
        if (derConvertFacade == null)
            throw new ArgumentNullException(nameof(derConvertFacade));

        if (string.IsNullOrEmpty(der))
            throw new ArgumentNullException(nameof(der));

        var keyBytes = derConvertFacade.ToDer(der);
        return new PublicKey(keyBytes);
    }

    public static PublicKey FromDer(byte[] der, IDerConvertFacade derConvertFacade)
    {
        if (derConvertFacade == null)
            throw new ArgumentNullException(nameof(derConvertFacade));

        if (der == null || der.Length == 0)
            throw new ArgumentNullException(nameof(der));

        var keyBytes = derConvertFacade.ToDer(der);
        return new PublicKey(keyBytes);
    }

    public static PublicKey FromPem(string pem, IPemConvertFacade pemConvertFacade)
    {
        if (pemConvertFacade == null)
            throw new ArgumentNullException(nameof(pemConvertFacade));

        if (string.IsNullOrEmpty(pem))
            throw new ArgumentNullException(nameof(pem));

        var keyBytes = pemConvertFacade.ToPem(pem);
        return new PublicKey(keyBytes);
    }

    public static PublicKey FromPem(byte[] pem, IPemConvertFacade pemConvertFacade)
    {
        if (pemConvertFacade == null)
            throw new ArgumentNullException(nameof(pemConvertFacade));

        if (pem == null || pem.Length == 0)
            throw new ArgumentNullException(nameof(pem));

        var keyBytes = pemConvertFacade.ToPem(pem);
        return new PublicKey(keyBytes);
    }
}
