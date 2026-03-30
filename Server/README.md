
---

# Main Server

This is the main server for coordinating encrypted messages for the **Clamshell Project**. It consists of three primary components: the database, the API, and the receiver. Together, they manage message storage, retrieval, validation, and display.

---

## Database

The database runs in a **PostgreSQL Docker container** and stores all received messages in a single table defined in `database/init.sql`. Each entry includes:

* The **message content** and its **PGN** (Parameter Group Number)
* A **timestamp** indicating when the message was received
* Flags for **signed** and **validated** status to ensure authenticity
* Optional metadata, such as message type or source

The database ensures that all messages can be **verified** cryptographically and supports queries for analytics or live monitoring. Regular backups and indexing are recommended to maintain performance as message volume grows.

---

## API

The API provides RESTful endpoints for retrieving, filtering, and managing messages stored in the database. It interacts with the web interface to present information visually and programmatically.

**Endpoints include:**

* **Login** – `GET /login/{username}/{password}`: Validates user credentials
* **Status Check** – `GET /status`: Checks if the service is live
* **Get All Messages** – `GET /all`: Retrieves every stored message
* **Get Total Message Count** – `GET /total`: Returns the total number of messages
* **Get New Messages Since Time** – `GET /new/{time}`: Retrieves messages received after a specific timestamp
* **Get Messages by PGN** – `GET /by-pgn/{pgn}`: Filters messages by PGN
* **Get Messages by PGN Since Time** – `GET /by-pgn/{pgn}/since/{time}`: Filters messages by PGN and timestamp
* **Get Signed Messages** – `GET /signed`: Returns only messages that are signed
* **Get Unsigned Messages** – `GET /unsigned`: Returns messages without a valid signature
* **Get Validated Messages** – `GET /validated`: Returns messages that have passed validation checks
* **Get Unvalidated Messages** – `GET /unvalidated`: Returns messages pending validation
* **Encryption Status** – `GET /encryptionStatus`: Returns partial key and nonce for debugging
* **Reset Table (Testing Only)** – `GET /Reset`: Clears the messages table for testing

The API is **stateless** and does not require persistent sessions. Responses are returned in JSON format, and all endpoints return HTTP `200 OK`. Authentication is lightweight, primarily for administrative access.

---

## The Receiver

The **receiver** is responsible for accepting encrypted TCP packets from nodes. It is an **asynchronous process** capable of handling multiple simultaneous connections. Its core responsibilities include:

1. Decrypting messages using XChaCha20-Poly1305 encryption
2. Validating signatures to ensure authenticity
3. Storing messages in the database after verification
4. logging errors or invalid messages for monitoring

This design allows nodes to push messages in real-time while ensuring that all stored data is secure and trustworthy. Scalability is achieved through async handling and optimized database writes.

---
