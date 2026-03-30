# Messages API

A REST API for retrieving and managing messages, including signed/unsigned and validated/unvalidated data.

---

## Base URL

```
http://<host>:<port>/api/messages
```

Replace `<host>` and `<port>` with your server configuration.

---

## Endpoints

### 1. Login

**GET** `/login/{username}/{password}`
**Description:** Validate user login.

```bash
curl -X GET "http://localhost:5000/api/messages/login/admin/password123"
```

**Response:**

```json
true
```

**Example (invalid login):**

```bash
curl -X GET "http://localhost:5000/api/messages/login/admin/wrongpass"
```

```json
false
```

---

### 2. Status Check

**GET** `/status`
**Description:** Check if the service is live.

```bash
curl -X GET "http://localhost:5000/api/messages/status"
```

**Response:**

```json
true
```

---

### 3. Get All Messages

**GET** `/all`

```bash
curl -X GET "http://localhost:5000/api/messages/all"
```

**Example Response:**

```json
[
  {
    "id": 1,
    "pgn": "01F112",
    "data": "ABC123",
    "timestamp": 1710000000,
    "signed": true,
    "validated": true
  }
]
```

---

### 4. Get Total Message Count

**GET** `/total`

```bash
curl -X GET "http://localhost:5000/api/messages/total"
```

**Example Response:**

```json
42
```

---

### 5. Get New Messages Since Time

**GET** `/new/{time}`

```bash
curl -X GET "http://localhost:5000/api/messages/new/60"
```

**Example Response:**

```json
[
  {
    "id": 5,
    "pgn": "01F113",
    "timestamp": 1710000060
  }
]
```

---

### 6. Get Messages by PGN

**GET** `/by-pgn/{pgn}`

```bash
curl -X GET "http://localhost:5000/api/messages/by-pgn/01F112"
```

**Example Response:**

```json
[
  {
    "id": 1,
    "pgn": "01F112",
    "data": "ABC123"
  }
]
```

---

### 7. Get Messages by PGN Since Time

**GET** `/by-pgn/{pgn}/since/{time}`

```bash
curl -X GET "http://localhost:5000/api/messages/by-pgn/01F112/since/60"
```

**Example Response:**

```json
[
  {
    "id": 6,
    "pgn": "01F112",
    "timestamp": 1710000100
  }
]
```

---

### 8. Get Signed Messages

**GET** `/signed`

```bash
curl -X GET "http://localhost:5000/api/messages/signed"
```

**Example Response:**

```json
[
  {
    "id": 2,
    "signed": true
  }
]
```

---

### 9. Get Unsigned Messages

**GET** `/unsigned`

```bash
curl -X GET "http://localhost:5000/api/messages/unsigned"
```

**Example Response:**

```json
[
  {
    "id": 3,
    "signed": false
  }
]
```

---

### 10. Get Validated Messages

**GET** `/validated`

```bash
curl -X GET "http://localhost:5000/api/messages/validated"
```

**Example Response:**

```json
[
  {
    "id": 2,
    "validated": true
  }
]
```

---

### 11. Get Unvalidated Messages

**GET** `/unvalidated`

```bash
curl -X GET "http://localhost:5000/api/messages/unvalidated"
```

**Example Response:**

```json
[
  {
    "id": 4,
    "validated": false
  }
]
```

---

### 12. Encryption Status

**GET** `/encryptionStatus`
**Description:** Returns the last 10 characters of the key and nonce (hex).

```bash
curl -X GET "http://localhost:5000/api/messages/encryptionStatus"
```

**Example Response:**

```json
{
  "key": "A1B2C3D4E5",
  "nonce": "F6G7H8I9J0"
}
```

---

### 13. Reset Table (Testing Only)

**GET** `/Reset`

```bash
curl -X GET "http://localhost:5000/api/messages/Reset"
```

**Example Response:**

```json
true
```

---

## Environment Variables

Required:

* `DEFAULTUSERNAME`
* `DEFAULTPASSWORD`
* `XCHACHA20POLY1305_KEY` (hex string)
* `XCHACHA20POLY1305_NONCE` (hex string)

---

## Notes

* All responses return HTTP `200 OK`
* Authentication is stateless
* Encryption values are partially exposed for debugging only
* Reset endpoint should not be enabled in production

---
