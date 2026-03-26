# Messages API

A simple ASP.NET Core Web API for retrieving and managing message data.
All endpoints are prefixed with:

```
/api/messages
```

---

## Authentication

### Validate Login

Checks if provided credentials match the default login.

**Endpoint**

```
GET /api/messages/login/{username}/{password}
```

**Example**

```bash
curl http://localhost:5000/api/messages/login/admin/admin
```

**Response**

```json
true
```

---

## Get All Messages

Returns all stored messages.

**Endpoint**

```
GET /api/messages/all
```

**Example**

```bash
curl http://localhost:5000/api/messages/all
```

---

## Get Total Message Count

Returns the total number of messages.

**Endpoint**

```
GET /api/messages/total
```

**Example**

```bash
curl http://localhost:5000/api/messages/total
```

**Response**

```json
42
```

---

## Get New Messages

Returns messages newer than a given time value.

**Endpoint**

```
GET /api/messages/new/{time}
```

**Parameters**

* `time` (int): Timestamp or time threshold

**Example**

```bash
curl http://localhost:5000/api/messages/new/60
```

---

## Get Messages by PGN

Returns messages matching a specific PGN.

**Endpoint**

```
GET /api/messages/by-pgn/{pgn}
```

**Example**

```bash
curl http://localhost:5000/api/messages/by-pgn/01F801
```

---

## Get Messages by PGN Since Time

Returns messages for a PGN after a given time.

**Endpoint**

```
GET /api/messages/by-pgn/{pgn}/since/{time}
```

**Example**

```bash
curl http://localhost:5000/api/messages/by-pgn/01F801/since/60
```

---

## Get Signed Messages

Returns messages that passed signature validation.

**Endpoint**

```
GET /api/messages/signed
```

**Example**

```bash
curl http://localhost:5000/api/messages/signed
```

---

## Get Unsigned Messages

Returns messages that failed signature validation.

**Endpoint**

```
GET /api/messages/unsigned
```

**Example**

```bash
curl http://localhost:5000/api/messages/unsigned
```

---

## Reset Database (Testing Only)

Clears or resets the message table.

⚠️ **Warning:** This is intended for testing only.

**Endpoint**

```
GET /api/messages/reset
```

**Example**

```bash
curl http://localhost:5000/api/messages/reset
```

---

## Notes

* All endpoints use HTTP GET.
* No authentication beyond the simple login endpoint is enforced.
* Responses are returned using `Ok(...)` and may vary depending on database implementation.
* PGN values are expected as hexadecimal strings.

---

## Example Base URL

```
http://localhost:5000/api/messages
```

Adjust the port and host as needed for your environment.
