# Vehicle Explorer

Vehicle Explorer is a .NET 10 web application that allows users to search for vehicle models by manufacturer, model year, and vehicle type.

Vehicle data is retrieved from the [NHTSA Vehicle Product Information Catalog (vPIC) API](https://vpic.nhtsa.dot.gov/api/).

## Live Demo

The application is deployed on AWS EC2:

http://18.184.96.154

> **Note:** The demo currently uses HTTP only. If your browser automatically switches to HTTPS, please use the `http://` URL explicitly.

## Features

- Browse and search vehicle manufacturers
- Select a model year from 1980 to the current year
- Browse vehicle types available for a selected manufacturer
- Search for matching vehicle models
- Built-in API playground for testing application endpoints
- Validation and centralized error handling
- Application logging
- Unit tests
- Docker support

## Technology Stack

- .NET 10
- ASP.NET Core Web API
- HTML
- CSS
- JavaScript
- xUnit
- Docker
- NHTSA vPIC API
- AWS EC2

## Project Structure

```text
vehicle-explorer/
├── src/
│   └── VehicleExplorer.Api/
│       ├── Controllers/
│       ├── Exceptions/
│       ├── Models/
│       ├── Services/
│       └── wwwroot/
├── tests/
│   └── VehicleExplorer.Tests/
├── Dockerfile
├── .dockerignore
├── VehicleExplorer.sln
└── README.md
```

## Prerequisites

To run the application locally, install:

- .NET 10 SDK
- Git

Docker is optional if you want to run the containerized version.

## Running Locally

Clone the repository:

```bash
git clone https://github.com/qarqaz/vehicle-explorer.git
cd vehicle-explorer
```

Restore dependencies:

```bash
dotnet restore
```

Run the application:

```bash
dotnet run --project src/VehicleExplorer.Api
```

The application will display its local URLs in the terminal. Open the HTTPS URL in your browser.

For example:

```text
https://localhost:7151
```

The exact port may differ depending on your local configuration.

## Running Tests

From the repository root:

```bash
dotnet test
```

The project currently contains unit tests covering request validation, controller behavior, and NHTSA response handling.

## Running with Docker

Build the Docker image from the repository root:

```bash
docker build -t vehicle-explorer .
```

Run the container:

```bash
docker run --rm -p 8080:8080 --name vehicle-explorer-container vehicle-explorer
```

Open:

```text
http://localhost:8080
```

Stop the container with `Ctrl+C`.

## API Endpoints

> **Quick API Testing:** After running the application, you can test all API endpoints directly using `src/VehicleExplorer.Api/VehicleExplorer.Api.http`. No additional API testing tool is required.

### Get Vehicle Makes

```http
GET /api/makes
```

### Get Vehicle Types for a Make

```http
GET /api/makes/{makeId}/vehicle-types
```

Example:

```http
GET /api/makes/448/vehicle-types
```

### Get Vehicle Models

```http
GET /api/models?makeId={makeId}&year={year}&vehicleType={vehicleType}
```

Example:

```http
GET /api/models?makeId=448&year=2015&vehicleType=Truck
```

## External API

Vehicle Explorer uses the NHTSA vPIC API as its vehicle data source.

The application communicates with NHTSA through a typed `HttpClient`. External NHTSA response models are kept separate from the response models exposed by the application API.

The NHTSA base URL is configured in `appsettings.json`.

## Error Handling and Validation

The application includes:

- Validation for make ID, model year, and vehicle type
- Automatic ASP.NET Core validation responses
- Centralized exception handling
- `ProblemDetails` error responses
- Handling for NHTSA HTTP, timeout, and JSON errors
- Structured application logging

## Testing

The test project uses xUnit.

Current tests cover:

- Valid model search requests
- Invalid make IDs
- Future model years
- Controller sorting and mapping behavior
- Controller validation behavior
- NHTSA JSON deserialization through `VehicleService`