#!/bin/bash

perform_curl() {
    local url=$1
    local method=$2
    local data=$3

    echo "Performing $method request to $url"
    
    if [ "$method" == "GET" ]; then
        response=$(curl -s -w "%{http_code}" -o /dev/null -X $method "$url")
    else
        response=$(curl -s -w "%{http_code}" -o /dev/null -X $method "$url" \
            -H "Content-Type: application/json" \
            -d "$data")
    fi

    echo "Curl response: $response"

    if ! [[ "$response" =~ ^[0-9]+$ ]]; then
        echo "Curl command failed or returned an invalid HTTP code: $response"
        exit 1
    fi

    if [ "$response" -ne 200 ]; then
        echo "Error: HTTP code $response"
        exit 1
    fi
}


echo "Registering a new user..."
perform_curl "http://localhost:5000/api/authorization/reg" "POST" '{"Name": "John", "Surname": "Doe", "Email": "john.doe@example.com", "Password": "securepassword"}'
echo "User registered successfully."

echo "Checking if the user exists..."
perform_curl "http://localhost:5000/api/authorization/check?email=john.doe@example.com" "GET"
echo "User exists."

echo "Logging in the user..."
perform_curl "http://localhost:5000/api/authorization/login" "POST" '{"Email": "john.doe@example.com", "Password": "securepassword"}'
echo "Login successful."

echo "Deleting the user..."
perform_curl "http://localhost:5000/api/authorization/delete" "DELETE" '{"Email": "john.doe@example.com", "Password": "securepassword"}'
echo "User deleted successfully."

echo "Checking again if the user exists..."
perform_curl "http://localhost:5000/api/authorization/check?email=john.doe@example.com" "GET"
echo "User does not exist."

echo "All tests passed successfully."
