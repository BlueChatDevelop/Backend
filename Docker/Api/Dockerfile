# Use the official .NET SDK image to build and run the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project files to the container
COPY ./../../API/NetASP/. ./

# Restore dependencies
RUN dotnet restore

# Build the application
RUN dotnet build --configuration Release

# Run the application
CMD ["dotnet", "run", "--configuration", "Release", "--urls", "http://0.0.0.0:5000"]
