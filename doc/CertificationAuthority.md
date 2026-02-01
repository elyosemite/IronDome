# Certification Authority (CA) Service

This is the core PKI service. It handles Key Management (KMS) and Certificate Lifecycle Management (CLM).

## 2.1. Key Management
Manages the generation and safe storage of cryptographic key pairs.

### Generate Key Pair
Generates a new asymmetric key pair. The private key is securely stored (e.g., in an HSM or encrypted DB) and never exposed unless explicitly exported (if allowed).

*   **POST** `/api/v1/keys`

**Request Body:**
```json
{
  "ownerId": "uuid-of-identity",
  "algorithm": "RSA", // RSA, ECDSA, ED25519
  "keySize": 4096, // Optional, depending on algorithm (e.g., 2048, 4096, P-256, P-384)
  "usage": ["SIGNING", "ENCRYPTION"],
  "exportable": false
}
```

**Response:**
```json
{
  "keyId": "key-uuid",
  "publicKey": "base64-encoded-pem-or-der",
  "algorithm": "RSA",
  "createdAt": "2026-01-31T10:00:00Z"
}
```

### Get Public Key
*   **GET** `/api/v1/keys/{keyId}/public`

### Destroy Key Pair
Irreversibly destroys the key material.

*   **DELETE** `/api/v1/keys/{keyId}`

---

## 2.2. Authority Management
Manages the Certification Authorities themselves (Root and Intermediates).

### Create Certification Authority
Bootstraps a new CA. If it is a Root CA, it self-signs. If Intermediate, it must be signed by a parent.

*   **POST** `/api/v1/authorities`

**Request Body:**
```json
{
  "name": "IronDome Organization Root CA",
  "type": "ROOT", // ROOT, INTERMEDIATE
  "distinguishedName": {
    "cn": "IronDome Root CA",
    "o": "IronDome Corp",
    "c": "US"
  },
  "keyParameters": {
    "algorithm": "RSA",
    "keySize": 4096
  },
  "validityYears": 10,
  "parentAuthorityId": null // Required if type is INTERMEDIATE
}
```

### Get Authority Details
*   **GET** `/api/v1/authorities/{id}`

### Get Authority Certificate Chain
Retrieves the full chain of trust for a specific CA.

*   **GET** `/api/v1/authorities/{id}/chain`

---

## 2.3. Certificate Lifecycle
Issuance and revocation of certificates for end-entities.

### Issue Certificate (Internal Key)
Issues a certificate for a Key Pair already managed by this system.

*   **POST** `/api/v1/certificates/issue`

**Request Body:**
```json
{
  "authorityId": "ca-uuid",
  "keyId": "key-uuid-of-subject",
  "subjectDn": {
    "cn": "John Doe",
    "email": "jdoe@example.com"
  },
  "validityDays": 365,
  "extensions": {
    "keyUsage": ["DIGITAL_SIGNATURE", "KEY_ENCIPHERMENT"],
    "extendedKeyUsage": ["CLIENT_AUTH", "EMAIL_PROTECTION"],
    "basicConstraints": {
      "isCa": false
    }
  }
}
```

### Issue Certificate (CSR)
Issues a certificate based on an external Certificate Signing Request (CSR).

*   **POST** `/api/v1/certificates/issue-csr`

**Request Body:**
```json
{
  "authorityId": "ca-uuid",
  "csr": "-----BEGIN CERTIFICATE REQUEST...-----",
  "validityDays": 90,
  "template": "WEB_SERVER" // Optional: Apply a pre-defined profile
}
```

### Revoke Certificate
Revokes a certificate before its expiration date.

*   **POST** `/api/v1/certificates/{serialNumber}/revoke`

**Request Body:**
```json
{
  "authorityId": "ca-uuid",
  "reason": "KEY_COMPROMISE" // UNSPECIFIED, KEY_COMPROMISE, CA_COMPROMISE, AFFILIATION_CHANGED, SUPERSEDED, CESSATION_OF_OPERATION
}
```

### Get Certificate Status (OCSP-like)
Check validity of a specific certificate.

*   **GET** `/api/v1/certificates/{serialNumber}/status?authorityId={caId}`

### Download Certificate Revocation List (CRL)
*   **GET** `/api/v1/authorities/{id}/crl`
