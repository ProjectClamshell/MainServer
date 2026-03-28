# Messages API

A REST API for retrieving and managing messages, including signed/unsigned and validated/unvalidated data.

## Base URL

```
http://<host>:<port>/api/messages
```

Replace `<host>` and `<port>` with your server configuration.

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

---

### 4. Get Total Message Count

**GET** `/total`

```bash
curl -X GET "http://localhost:5000/api/messages/total"
```

---

### 5. Get New Messages Since Time

**GET** `/new/{time}`

```bash
curl -X GET "http://localhost:5000/api/messages/new/60"
```

---

### 6. Get Messages by PGN

**GET** `/by-pgn/{pgn}`

```bash
curl -X GET "http://localhost:5000/api/messages/by-pgn/01F112"
```

---

### 7. Get Messages by PGN Since Time

**GET** `/by-pgn/{pgn}/since/{time}`

```bash
curl -X GET "http://localhost:5000/api/messages/by-pgn/01F112/since/60"
```

---

### 8. Get Signed Messages

**GET** `/signed`

```bash
curl -X GET "http://localhost:5000/api/messages/signed"
```

---

### 9. Get Unsigned Messages

**GET** `/unsigned`

```bash
curl -X GET "http://localhost:5000/api/messages/unsigned"
```

---

### 10. Get Validated Messages

**GET** `/validated`

```bash
curl -X GET "http://localhost:5000/api/messages/validated"
```

---

### 11. Get Unvalidated Messages

**GET** `/unvalidated`

```bash
curl -X GET "http://localhost:5000/api/messages/unvalidated"
```

---

### 12. Reset Table (Testing Only)

**GET** `/Reset`

```bash
curl -X GET "http://localhost:5000/api/messages/Reset"
```

---
