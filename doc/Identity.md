# Identity Service

Responsible for managing the lifecycle of actors (users or systems) that participate in the PKI.

## 1.1. Actors

### Create Actor
Registers a new entity (User or System) in the ecosystem.

*   **POST** `/api/v1/identities`

**Request Body:**
```json
{
  "username": "jdoe",
  "email": "jdoe@example.com",
  "type": "PERSON", // PERSON, SYSTEM, DEVICE
  "metadata": {
    "department": "IT",
    "employeeId": "12345"
  }
}
```

### Get Actor Details
*   **GET** `/api/v1/identities/{id}`

### Update Actor Status
Used to suspend or activate an identity.

*   **PATCH** `/api/v1/identities/{id}/status`

**Request Body:**
```json
{
  "status": "SUSPENDED", // ACTIVE, SUSPENDED, REVOKED
  "reason": "Security investigation pending"
}
```
