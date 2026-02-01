# Digital Signature Service

A high-level service that uses the keys and certificates managed by the CA service to perform business operations.

## 3.1. Signing

### Sign Data
Signs a payload using a managed private key.

*   **POST** `/api/v1/signing/sign`

**Request Body:**
```json
{
  "keyId": "key-uuid", // Key must belong to the authenticated user
  "data": "base64-encoded-data",
  "hashingAlgorithm": "SHA256",
  "format": "CMS" // RAW, CMS (PKCS#7), JWS
}
```

**Response:**
```json
{
  "signature": "base64-encoded-signature",
  "format": "CMS",
  "certificateId": "cert-uuid-used-for-signing"
}
```

## 3.2. Verification

### Verify Signature
Verifies a signature against data and a certificate.

*   **POST** `/api/v1/signing/verify`

**Request Body:**
```json
{
  "data": "base64-encoded-original-data",
  "signature": "base64-encoded-signature",
  "certificate": "base64-encoded-cert", // Optional if certificateId is provided
  "certificateId": "cert-uuid" // Optional if certificate is provided
}
```

**Response:**
```json
{
  "valid": true,
  "errors": [],
  "signer": {
    "subject": "CN=John Doe, ...",
    "issuer": "CN=IronDome Root CA, ..."
  }
}
```
