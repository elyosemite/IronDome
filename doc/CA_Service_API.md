# Certification Authority (CA) Service API Documentation

**Responsibility:** Manages the lifecycle of Cryptographic Keys and X.509 Certificates. It trusts the **Identity Service** for subject data but enforces all cryptographic policies (RFC 5280, Key Usage, Path Length).

---

## 1. Workflows & Actors

*   **Root Authority**: A Subject elevated to be the Trust Anchor.
*   **Intermediate Authority**: A Subject elevated to issue certificates, signed by a Root or another Intermediate.
*   **End-Entity**: A Subject that holds a certificate for usage (TLS, Email, Signing) but cannot sign other certificates.

### 1.1. Hierarchy Creation Sequence

```mermaid
sequenceDiagram
    autonumber
    participant Admin as PKI Admin
    participant CASvc as CA Service
    participant IdentitySvc as Identity Service

    %% ROOT CREATION
    note over Admin, CASvc: 1. Creating the Root of Trust
    Admin->>CASvc: POST /authorities { "subjectId": "subj_alice_root", "type": "ROOT" }
    CASvc->>IdentitySvc: Get Subject Data (Alice)
    CASvc->>CASvc: Generate Key Pair + Self-Sign
    CASvc-->>Admin: Returns AuthID: root_ca_id

    %% INTERMEDIATE CREATION
    note over Admin, CASvc: 2. Delegating Authority (Intermediate)
    Admin->>CASvc: POST /authorities
    note right of Admin: { "subjectId": "subj_bob_manager", "type": "INTERMEDIATE", "parent": "root_ca_id" }
    CASvc->>IdentitySvc: Get Subject Data (Bob)
    CASvc->>CASvc: Generate Key Pair (Bob)
    CASvc->>CASvc: Sign Bob's Cert with Root Key
    note right of CASvc: Enforce PathLen > 0 (Manager)
    CASvc-->>Admin: Returns AuthID: interm_ca_id

    %% END-ENTITY ISSUANCE
    note over Admin, CASvc: 3. Issuing User Certificate
    Admin->>CASvc: POST /issuers/interm_ca_id/certificates
    note right of Admin: { "subjectId": "subj_charlie_user", "template": "TLS_CLIENT" }
    CASvc->>IdentitySvc: Get Subject Data (Charlie)
    CASvc->>CASvc: Sign Charlie's Cert with Intermediate Key
    note right of CASvc: Enforce BasicConstraints: CA=FALSE
    CASvc-->>Admin: Returns X.509 Certificate
```

### 1.2. Revocation Sequence

```mermaid
sequenceDiagram
    participant Admin
    participant CASvc as CA Service

    Admin->>CASvc: POST /certificates/{cert_id}/revocation
    note right of Admin: { "reason": "KEY_COMPROMISE" }
    CASvc->>CASvc: Mark Certificate as REVOKED in DB
    CASvc->>CASvc: Add Serial Number to CRL Pending List
    CASvc-->>Admin: 202 Accepted

    note over CASvc: Async Process (CRL Gen)
    CASvc->>CASvc: Generate new CRL signed by Issuer
    CASvc->>CASvc: Publish CRL to Distribution Point
```

---

## 2. Authority Management (CAs)

Transform a `Subject` (from Identity Service) into a **Certification Authority**. This involves generating a key pair and either self-signing (Root) or requesting a signature (Intermediate).

### 2.1. Promote Subject to Authority
Creates a new CA.

*   **POST** `/api/v1/authorities`

**Request (Scenario: Creating an Intermediate Issuer):**
```json
{
  "subjectId": "771f9511-f30c-52e5-b827-557766551111", // UUID of Maria Silva
  "type": "INTERMEDIATE", // ROOT, INTERMEDIATE
  "parentAuthorityId": "110e8400-e29b-41d4-a716-446655440000", // The Root CA ID
  "keySpec": {
    "algorithm": "RSA",
    "size": 4096
  },
  "policy": {
    "validityYears": 5,
    "pathLenConstraint": 0, // CRITICAL: 0 means Maria CANNOT create sub-CAs.
    "crlDistributionPoint": "http://pki.irondome.io/crl/root.crl",
    "ocspResponderUrl": "http://pki.irondome.io/ocsp"
  }
}
```

**Response (201 Created):**
```json
{
  "id": "992a0622-d41d-63f6-c938-668877662222",
  "subjectId": "771f9511-f30c-52e5-b827-557766551111",
  "type": "INTERMEDIATE",
  "status": "ACTIVE",
  "serialNumber": "4A1B2C3D4E5F6G7H",
  "thumbprint": "A1B2C3D4...",
  "notBefore": "2026-01-31T14:40:00Z",
  "notAfter": "2031-01-31T14:40:00Z",
  "hierarchy": {
    "isRoot": false,
    "pathLenConstraint": 0
  }
}
```

### 2.2. Get Authority Chain
Retrieves the full trust chain (P7B or ordered list) for installation.

*   **GET** `/api/v1/authorities/{id}/chain`

---

## 3. Certificate Issuance (End-Entities)

Issues certificates for non-authority subjects (Web Servers, Email, Signatures).

### 3.1. Issue Certificate
*   **POST** `/api/v1/issuers/{authorityId}/certificates`

**Request:**
```json
{
  "subjectId": "882f9511-f30c-52e5-b827-557766553333", // UUID of "Web Server 01"
  "template": "TLS_SERVER", // TLS_SERVER, TLS_CLIENT, CODE_SIGNING
  "validityDays": 365,
  "san": { // Subject Alternative Names
    "dnsNames": ["www.irondome.io", "api.irondome.io"],
    "ipAddresses": ["192.168.1.10"]
  }
}
```

**Response (200 OK):**
```json
{
  "id": "123f9511-f30c-52e5-b827-557766559999",
  "serialNumber": "9988776655443322",
  "certificateContent": "-----BEGIN CERTIFICATE-----\nMIIDAzCCAfugAwIBAgIQ...\n-----END CERTIFICATE-----",
  "format": "PEM"
}
```

---

## 4. Revocation (CRL)

### 4.1. Revoke Certificate
*   **POST** `/api/v1/certificates/{certId}/revocation`

**Request:**
```json
{
  "reason": "KEY_COMPROMISE", // UNSPECIFIED, KEY_COMPROMISE, CA_COMPROMISE, AFFILIATION_CHANGED, SUPERSEDED, CESSATION_OF_OPERATION
  "comment": "Server was decommissioned and hard drive was not wiped."
}
```

### 4.2. Download CRL
*   **GET** `/api/v1/issuers/{authorityId}/crl`
    *   Returns the raw binary DER or PEM CRL file.

