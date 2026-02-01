# IronDome System Concepts

This document outlines the core conceptual model for the **IronDome PKI** ecosystem. It defines the separation of concerns between legal entities, actors, and cryptographic capabilities, adhering to **RFC 5280**.

## 1. Core Data Model

To provide flexibility and clear responsibility boundaries, the system decouples "Identity" (Who is this?) from "Capability" (What can this do?).

### 1.1. Organization
Represents the **legal or logical container** for all operations. It provides the context for the `O` (Organization) and `C` (Country) fields in a Distinguished Name (DN).

*   **Role**: Legal boundary, billing entity, root of trust context.
*   **Attributes**: Legal Name, Tax ID, Country.

### 1.2. Subject (Identity)
Represents a distinct **actor** within an Organization. Formerly referred to as "UserProfile".
A `Subject` is the entity that *owns* a private key and is the *subject* of a certificate.

*   **Role**: The unique identity. Source of truth for `CN` (Common Name) and `OU` (Organizational Unit).
*   **Types**:
    *   **Person**: A human user (e.g., "Maria Silva").
    *   **System**: A service account or daemon.
    *   **Device**: IoT device or hardware component.

### 1.3. Authority (Capability)
Represents the **elevation of privilege** for a specific `Subject`.
A `Subject` by itself is just a user. To act as a Certification Authority (CA), an `Authority` resource must be created and linked to that Subject.

*   **Role**: Defines the cryptographic powers, issuance policies, and position in the trust hierarchy.
*   **Key Concept**: A Subject can exist without being an Authority. An Authority cannot exist without a Subject.

---

## 2. Diagrammatic Overview

### 2.1. Entity Relationships (Organizations & Subjects)

The following diagram illustrates the relationship between Organizations, Subjects, and the "Authority" overlay. Note that "End Users" are simply Subjects without an associated Authority.

```mermaid
erDiagram
    %% ORGANIZATIONS
    ORG_1 { string name "IronDome Corp" }
    ORG_2 { string name "Siberian CyberSec" }
    ORG_3 { string name "Beijing Tech" }

    %% SUBJECTS (7 Distinct Nationalities)
    SUBJ_RUS { string name "Dmitri Volkov" string role "Root Admin" }
    SUBJ_CHI { string name "Wei Zhang" string role "Manager" }
    SUBJ_USA { string name "John Smith" string role "Manager" }
    SUBJ_BRA { string name "Joao Silva" string role "Manager" }
    SUBJ_USA2 { string name "Sarah Connor" string role "Engineer" }
    SUBJ_BRA2 { string name "Ana Souza" string role "Auditor" }
    SUBJ_CHI2 { string name "Li Ming" string role "End User" }

    %% RELATIONSHIPS
    ORG_2 ||--o{ SUBJ_RUS : employs
    ORG_3 ||--o{ SUBJ_CHI : employs
    ORG_1 ||--o{ SUBJ_USA : employs
    ORG_1 ||--o{ SUBJ_BRA : employs
    ORG_1 ||--o{ SUBJ_USA2 : employs

    %% 'Ana Souza' and 'Li Ming' are independent contractors (No Org)
    %% Just to show Subjects can exist loosely or linked to other contexts

    %% AUTHORITIES (The Privilege Overlay)
    AUTH_ROOT { string type "ROOT" string pathLen "Unlimited" }
    AUTH_INT_1 { string type "INTERMEDIATE" string pathLen "1" }
    AUTH_INT_2 { string type "INTERMEDIATE" string pathLen "0" }
    AUTH_INT_3 { string type "INTERMEDIATE" string pathLen "0" }

    %% LINKING AUTHORITY TO SUBJECT
    SUBJ_RUS ||--|| AUTH_ROOT : "elevated to"
    SUBJ_CHI ||--|| AUTH_INT_1 : "elevated to"
    SUBJ_USA ||--|| AUTH_INT_2 : "elevated to"
    SUBJ_BRA ||--|| AUTH_INT_3 : "elevated to"
```

### 2.2. Operation Sequence: From Zero to Certificate

This sequence separates the roles of the **Organization Admin** (who bootstraps the entities) and the **PKI Operator** (who manages the certificates).

```mermaid
sequenceDiagram
    autonumber

    box "Session A: Organization Admin" #f9f9f9
        participant OrgAdmin as Organization Administrator
    end

    box "Session B: PKI Operator" #e6f7ff
        participant PKIOp as PKI Operator/User
    end

    participant IdentitySvc as Identity Service
    participant CASvc as CA Service

    %% SESSION A: BOOTSTRAPPING
    rect rgb(240, 240, 240)
        note right of OrgAdmin: Goal: Register Legal Entities & Employees
        OrgAdmin->>IdentitySvc: POST /orgs { "name": "Siberian CyberSec" }
        IdentitySvc-->>OrgAdmin: 201 Created (OrgID: 100)

        OrgAdmin->>IdentitySvc: POST /orgs/100/subjects { "name": "Dmitri Volkov", "role": "Root Admin" }
        IdentitySvc-->>OrgAdmin: 201 Created (SubjID: dmitri)

        OrgAdmin->>IdentitySvc: POST /orgs/100/subjects { "name": "Wei Zhang", "role": "Manager" }
        IdentitySvc-->>OrgAdmin: 201 Created (SubjID: wei)

        OrgAdmin->>IdentitySvc: POST /subjects { "name": "Li Ming", "type": "EndUser" }
        IdentitySvc-->>OrgAdmin: 201 Created (SubjID: li_ming)
    end

    %% SESSION B: PKI OPERATIONS
    rect rgb(230, 247, 255)
        note right of PKIOp: Goal: Build Trust Hierarchy & Issue Certs

        %% 2. CREATING THE ROOT CA
        note over PKIOp, CASvc: Step 1: Promote Dmitri to Root CA
        PKIOp->>CASvc: POST /authorities { "subjectId": "dmitri", "type": "ROOT" }
        CASvc->>IdentitySvc: GET /subjects/dmitri (Verify Identity)
        CASvc->>CASvc: Generate Key Pair + Self-Sign
        CASvc-->>PKIOp: 201 Created (AuthID: auth_root)

        %% 3. CREATING INTERMEDIATE CA
        note over PKIOp, CASvc: Step 2: Promote Wei to Intermediate CA (Signed by Root)
        PKIOp->>CASvc: POST /authorities
        note right of PKIOp: { "subjectId": "wei", "type": "INTERMEDIATE", "parent": "auth_root" }
        CASvc->>IdentitySvc: GET /subjects/wei
        CASvc->>CASvc: Generate Key Pair for Wei
        CASvc->>CASvc: Sign Wei's Cert using Dmitri's Key
        note right of CASvc: Enforce PathLen logic here
        CASvc-->>PKIOp: 201 Created (AuthID: auth_wei)

        %% 4. ISSUING END-ENTITY CERTIFICATE
        note over PKIOp, CASvc: Step 3: Issue Final Cert for Li Ming (Signed by Wei)
        PKIOp->>CASvc: POST /issuers/auth_wei/certificates
        note right of PKIOp: { "subjectId": "li_ming", "template": "TLS_CLIENT" }
        CASvc->>IdentitySvc: GET /subjects/li_ming
        CASvc->>CASvc: Verify Wei's Authority status
        CASvc->>CASvc: Sign Li Ming's Cert using Wei's Key
        note right of CASvc: BasicConstraints: CA=False
        CASvc-->>PKIOp: 200 OK (X.509 Certificate)
    end
```

---

## 3. Authority Roles & Privilege Elevation

When a Subject is promoted to an Authority, specific cryptographic constraints are applied based on the desired role.

### 3.1. Root Authority
*   **Definition**: A Trust Anchor. The Subject holds a self-signed certificate.
*   **Capabilities**: Can issue certificates to anyone (other CAs or End-Entities).
*   **Configuration**: Unlimited path length (usually).

### 3.2. Intermediate Authority
*   **Definition**: A CA whose trust is derived from a parent (Root or another Intermediate).
*   **Capabilities**: Depends strictly on the **Path Length Constraint** assigned by the parent.

---

## 4. Cryptographic Enforcement (RFC 5280)

IronDome strictly enforces hierarchy rules using the X.509 **Basic Constraints (OID 2.5.29.19)** extension. This ensures that business rules defined in the `Authority` resource are respected by any standard PKI client globally.

### 4.1. The "PathLen" Rule

The power of an Intermediate CA is controlled by the `pathLenConstraint` field in its certificate.

#### Scenario A: Intermediate Manager (Delegated CA)
*   **Goal**: Maria Silva (Regional Manager) needs to create sub-CAs for her departments.
*   **Policy**: `pathLen > 0` (e.g., 1).
*   **Result**: Maria allows the creation of a chain: `Root -> Maria (Len=1) -> Dept IT (Len=0) -> WebServer`.
*   **Cryptographic Enforcement**: The certificate issued to Maria will have `BasicConstraints: cA=TRUE, pathLen=1`.

#### Scenario B: Intermediate Issuer (Terminal CA)
*   **Goal**: The "Dept IT" CA needs to issue SSL certificates but **must not** be able to create new CAs.
*   **Policy**: `pathLen = 0`.
*   **Result**: The chain stops here regarding authority. `Dept IT` can only sign End-Entity certificates.
*   **Cryptographic Enforcement**: The certificate issued to Dept IT will have `BasicConstraints: cA=TRUE, pathLen=0`.

---

## 5. Policy Matrix Summary

| Role | Resource Type | BasicConstraints `cA` | `pathLenConstraint` | Can Sign CAs? | Can Sign End-Entities? |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Root** | Authority | `TRUE` | `None` / `High` | Yes | Yes |
| **Manager** | Authority | `TRUE` | `> 0` | Yes | Yes |
| **Issuer** | Authority | `TRUE` | `0` | **NO** | Yes |
| **User/Device**| Subject (No Auth) | `FALSE` | `None` | No | No |
