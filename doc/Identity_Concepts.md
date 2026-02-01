# Identity Service - Conceptual Modeling

This document outlines the conceptual model for the **Identity Service**, focusing on the relationship between User Profiles and their roles within the Public Key Infrastructure (PKI), strictly adhering to **RFC 5280**.

## 1. User Profile

The `UserProfile` represents a distinct legal or logical entity within the system (e.g., "Maria Silva", a Web Server, or an Organization). It acts as the **owner** of cryptographic keys and the **subject** of certificates.

### Core Attributes
*   **Identity Data**: Name, Email, Organization Unit (e.g., "Maria Silva").
*   **Authentication**: Credentials (managed separately, but linked).
*   **Lifecycle**: Active, Suspended, Revoked.

---

## 2. Identity as a Certification Authority (CA)

In IronDome, a `UserProfile` can be elevated to possess **Authority Capabilities**. This elevation is not just a database flag; it dictates the cryptographic extensions of the certificates issued to that profile.

### 2.1. Authority Roles

The system distinguishes between three distinct capabilities:

1.  **Root Authority**:
    *   Trust Anchor. Self-signed.
    *   Holds the power to define the maximum depth of the entire PKI tree.

2.  **Intermediate Authority**:
    *   Trust derived from a parent CA.
    *   Can be an **Issuer** (signs final users) or a **Manager** (signs other CAs).

3.  **End-Entity (Final)**:
    *   Consumer only. Cannot sign other certificates.

---

## 3. Cryptographic Enforcement (RFC 5280)

To control the hierarchy, the Identity and Certificate services must manipulate the **Basic Constraints (OID 2.5.29.19)** extension.

### 3.1. Basic Constraints Fields

*   **cA (Boolean)**: Must be `TRUE` for any profile that needs to sign other certificates.
*   **pathLenConstraint (Integer)**: Defines the maximum number of **non-self-issued intermediate certificates** that may follow this certificate in a valid certification path.

### 3.2. Technical Scenarios for Intermediates

The parent CA determines the child's issuance power at the moment of signing by setting the `pathLenConstraint`.

#### Scenario A: The "Intermediate Issuer" (Terminal CA)
The profile is an authority but is restricted from creating further sub-authorities. It can only issue certificates to end-entities (users, devices).
*   **X.509 Mapping**: `cA=TRUE`, `pathLen=0`.
*   **Key Usage**: Must include `keyCertSign`.
*   **Result**: Any certificate signed by this profile **must** have `cA=FALSE`.

#### Scenario B: The "Intermediate Manager" (Delegated CA)
The profile is allowed to delegate authority to other sub-CAs.
*   **X.509 Mapping**: `cA=TRUE`, `pathLen=N` (where N > 0).
*   **Key Usage**: Must include `keyCertSign`.
*   **Result**: This profile can sign certificates where `cA=TRUE`, as long as the new `pathLen` is less than the current one.

---

## 4. Configuration and Issuance Policy

The `UserProfile` must define its **Issuance Policy**, which the Certificate Service will enforce cryptographically.

### 4.1. Policy Matrix

| Policy Type | Capability | `cA` | `pathLen` | `keyUsage` |
| :--- | :--- | :--- | :--- | :--- |
| **Root** | Full Control | `TRUE` | `None` or `High` | `keyCertSign`, `cRLSign` |
| **Manager CA** | Delegate Power | `TRUE` | `> 0` | `keyCertSign`, `cRLSign` |
| **Issuer CA** | End-User Only | `TRUE` | `0` | `keyCertSign`, `cRLSign` |
| **End-Entity** | Usage Only | `FALSE` | `None` | `digitalSignature`, etc. |

### 4.2. Implementation Rules

1.  **Validation**: When an Intermediate CA attempts to issue a certificate, the system must verify its own `pathLen`. If its `pathLen` is `0`, it is prohibited from issuing any certificate where `cA=TRUE`.
2.  **Criticality**: The `BasicConstraints` extension **must** be marked as **Critical** in all CA certificates. This ensures that any software validating the chain (browsers, clients) will reject the path if it cannot process or if the path length is exceeded.
3.  **Logical & Cryptographic Sync**: The Identity Service database stores the policy for business logic (UI/API restrictions), but the Certificate Service ensures the final X.509 artifact carries the enforcement globally.