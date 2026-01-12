# api2spec-fixture-fastendpoints

A FastEndpoints (.NET 8) API fixture for testing api2spec.

## Endpoints

- `GET /health` - Health check
- `GET /health/ready` - Readiness check
- `GET /users` - List users
- `GET /users/{id}` - Get user by ID
- `POST /users` - Create user
- `PUT /users/{id}` - Update user
- `DELETE /users/{id}` - Delete user
- `GET /posts` - List posts
- `GET /posts/{id}` - Get post by ID
- `POST /posts` - Create post
- `GET /users/{userId}/posts` - Get posts by user

## Running

```bash
docker compose up --build
```

The API will be available at http://localhost:8080
