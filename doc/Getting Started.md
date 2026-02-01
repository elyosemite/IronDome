# IronDome Overview

This document defines a RESTful API design for a **Public Key Infrastructure (PKI)** ecosystem compliant with **RFC 5280**, implemented in **C# using BouncyCastle**.

The goal is to provide **strong responsibility boundaries**, **security-first design**, and **long-term extensibility** for cryptographic services that are foundational to the platform.

## Architectural Principles

*   **RESTful Resources**: Predictable resource-oriented URLs.
*   **Statelessness**: No client session state stored on the server.
*   **Immutability**: Cryptographic artifacts (Keys, Certificates, Logs) are immutable once created.
*   **Standardization**: Adherence to RFC 5280 (Profiles), RFC 5019 (OCSP), and JSON Web Algorithms (JWA).
*   **Security**: Private keys are isolated; sensitive operations require strict authorization.

## Core Services Documentation

Please refer to the detailed documentation for each service:

1.  **[Identity Service](Identity.md)**: Manages users, authentication, and access policies.
2.  **[Certification Authority (CA) Service](CertificationAuthority.md)**: Manages the lifecycle of cryptographic keys and X.509 certificates (Issuance, Revocation, Storage).
3.  **[Digital Signature Service](DigitalSignature.md)**: Provides high-level signing and verification operations, abstracting complexity.
