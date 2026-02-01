# User Story: Identity Service Core Implementation

**ID**: US-01
**Title**: Implement Core Identity Management (Organization & Subject)
**Status**: Ready for Implementation

## 1. Goal
As a System Administrator, I want to register **Organizations** and **Subjects** (Users/Systems) in the Identity Service so that they can act as legal entities and actors within the PKI ecosystem.

## 2. Context & Concepts
This implementation serves as the foundational "Source of Truth" for the entire IronDome PKI. It must strictly adhere to the concepts defined in:
*   `doc/concepts.md` (Organization vs. Subject separation)
*   `doc/Identity_API.md` (Contract definitions)

### 2.1. Architectural Constraints
*   **Pattern**: Vertical Slice / Feature Folder (using FastEndpoints).
*   **Layers**:
    *   `Identity.Domain`: Pure domain logic, Entities, Value Objects.
    *   `Identity.Infrastructure`: Database context (EF Core), Repositories, Migrations.
    *   `Identity.Presentation`: API Endpoints (FastEndpoints), Configuration.
    *   `Identity.Test`: Unit and Integration tests.
*   **Forbidden**: Do NOT use a separate `Identity.Application` class library. Orchestration logic should reside in the **Endpoints** or **Domain Services**.
*   **Event Handling**: If domain events are needed (e.g., `SubjectCreated`), use the custom `IDomainEventDispatcher` pattern (without MediatR) as discussed.

## 3. Acceptance Criteria

### 3.1. Domain Modeling
*   **Organization Entity**:
    *   Attributes: `Id` (Guid), `LegalName`, `TradeName`, `TaxId` (CNPJ/EIN), `Country` (ISO 3166), `Address` (ValueObject).
    *   Behavior: Validation of mandatory fields.
*   **Subject Entity**:
    *   Attributes: `Id` (Guid), `OrganizationId` (FK), `CommonName`, `Email`, `Type` (Enum: Person, System, Device), `Department`.
    *   Behavior: `Email` must be unique per Organization. `CommonName` is required.


### 3.2. Infrastructure
*   Use **Entity Framework Core** with **PostgreSQL**.
*   Implement `Ardalis.Specification` for query logic if complex filtering is needed.
*   Configure **Entity Configurations** (Fluent API) to map `Subject` and `Organization` correctly.
*   You never MUST persist a domain model in the database. You have to create data model and create mapping from domain model to data model

### 3.3. API Endpoints (FastEndpoints)
Implement the following endpoints in `Identity.Presentation`.

#### A. Create Organization
*   **Route**: `POST /api/v1/organizations`
*   **Request**:
    ```json
    {
      "legalName": "IronDome Corp",
      "taxId": "12.345.678/0001-90",
      "country": "BR",
      "contactEmail": "admin@irondome.io"
    }
    ```
*   **Response**: `201 Created` with Organization ID.
*   **Validation**: `LegalName` and `TaxId` are mandatory.

#### B. Get Organization
*   **Route**: `GET /api/v1/organizations/{id}`
*   **Response**: Full Organization details.

#### C. Create Subject
*   **Route**: `POST /api/v1/organizations/{orgId}/subjects`
*   **Request**:
    ```json
    {
      "commonName": "Maria Silva",
      "email": "maria@irondome.io",
      "type": "PERSON",
      "department": "IT"
    }
    ```
*   **Response**: `201 Created` with Subject ID and computed DN attributes.
*   **Validation**: `orgId` must exist. `Email` format validation.

#### D. Get Subject
*   **Route**: `GET /api/v1/subjects/{id}`
*   **Response**: Subject details including `Organization` summary.

### 3.4. Testing
*   Create **Integration Tests** using `WebApplicationFactory` to verify the full flow (Endpoint -> DB -> Response).
*   Create **Unit Tests** for `Subject` domain logic (e.g., preventing empty names).

## 4. Technical Specifications

### 4.1. Project Structure (To Be Created)
Ensure the solution `src/Identity/Identity.sln` (or added to main SLN) contains:
```text
src/Identity/
├── Identity.Domain/           (Class Library)
├── Identity.Infrastructure/   (Class Library)
├── Identity.Presentation/     (Web API)
└── Identity.Test/             (nUnit)
```

### 4.2. Nuget Packages
*   `FastEndpoints`
*   `FastEndpoints.Swagger`
*   `Microsoft.EntityFrameworkCore.PostgreSQL`
*   `Ardalis.SharedKernel` (for EntityBase, DomainEventBase)
*   `Ardalis.Specification` (optional, for clean queries)

## 5. Implementation Steps (AI Guide)
1.  **Scaffold Projects**: Create the missing `.csproj` files and add them to the Solution.
2.  **Domain**: Implement `Organization` and `Subject` inheriting from `EntityBase<Guid>`.
3.  **Infra**: Setup `IdentityDbContext` and configure connection string.
4.  **Presentation**: Setup `Program.cs` with `AddFastEndpoints()`.
5.  **Endpoints**: Implement the 4 endpoints defined above.
6.  **Migration**: Create initial EF Core migration.
7.  **Verify**: Run tests.
