# ClamShellServer API Documentation

Base URL: `http://localhost:5000/api/messages`

---

## Endpoints

### Get New Messages
Returns any message posted in the last 5 minutes.

```
Get /api/messages/new
```

### Get Total Messages
Returns the total count of all messages in the database.

```
GET /api/messages/total
```

**Response**
```json
42
```

---

### Get Signed Messages
Returns the count of all messages where `signed = true`.

```
GET /api/messages/signed
```

**Response**
```json
30
```

---

### Get Unsigned Messages
Returns the count of all messages where `signed = false`.

```
GET /api/messages/unsigned
```

**Response**
```json
12
```

---

### Create New Message
Saves a new message to the database with the current timestamp.

```
POST /api/messages
Content-Type: application/json
```

**Request Body**
```json
"your message content here"
```

**Response**
```json
"saved"
```

---

## Example curl Commands

```bash
# Get new messages
curl http://localhost:5000/api/messages/new

# Get total messages
curl http://localhost:5000/api/messages/total

# Get signed messages
curl http://localhost:5000/api/messages/signed

# Get unsigned messages
curl http://localhost:5000/api/messages/unsigned

# Post a new message
curl -X POST http://localhost:5000/api/messages \
  -H "Content-Type: application/json" \
  -d '"your message here"'
```

---

## Notes
- All responses return HTTP `200 OK` on success.
- `received_at` is set automatically to the current UTC time on insert.
- `signed` defaults to `false` on insert.