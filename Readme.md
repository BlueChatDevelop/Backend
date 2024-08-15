# BlueChat API

This project provides a simple API for user registration, login, and management. The API is built using .NET and uses MySQL as the database.

## API Endpoints

### 1. Check if an email is registered
- **Path**: `api/authorization/check`
- **Method**: `GET`
- **Query Parameters**:
    - `email` (string): The email address to check.
- **Response**: Returns `true` if the email is registered, otherwise `false`.

### 2. Register a new user
- **Path**: `api/authorization/reg`
- **Method**: `POST`
- **Body**:
    - `Name` (string): The first name of the user.
    - `Surname` (string): The last name of the user.
    - `Email` (string): The email address of the user.
    - `Password` (string): The password for the user.
- **Response**: Returns `true` if the registration request is successful.

### 3. Login a user
- **Path**: `api/authorization/login`
- **Method**: `POST`
- **Body**:
    - `Email` (string): The email address of the user.
    - `Password` (string): The password for the user.
- **Response**: Returns `true` if the login request is successful.

### 4. Delete a user
- **Path**: `api/authorization/delete`
- **Method**: `DELETE`
- **Body**:
    - `Email` (string): The email address of the user.
    - `Password` (string): The password for the user.
- **Response**: Returns `true` if the delete request is successful.

## How to Run the Project

To run the project, navigate to the `Docker` directory and use Docker Compose to start the containers.

### Step 1: Start the Containers

Run the following command to start the containers in detached mode:

```bash
docker-compose up -d
```
### Step 2: Handling Errors
If the API returns a 500 Internal Server Error, you may need to restart the containers with a clean database state.
Use the following commands:
``` bash
docker-compose down -v
docker-compose up
```
This will remove all volumes, ensuring that the database is reinitialized, and then start the containers again.
### Troubleshooting
- **500 Internal Server Error**: If you encounter this error, try restarting the containers as described in Step 2 of the "How to Run the Project" section.
- **Access Issues**: Ensure that the API is accessible at `http://localhost:5000` or the corresponding IP address of your machine.
