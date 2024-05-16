# CapitalProject

This is a .NET 8 application that leverages Azure Cosmos DB to store and retrieve custom questions created by users. Below, you will find the necessary configurations and dependencies.

## Technologies

- **.NET 8**: Leverages newer features such as implicit usings, a deprecated startup class, and robust reflection properties.
- **Clean Architecture**: Maintains core ideas of clean architecture principles while considering project size.
- **Azure Cosmos DB**: Provides a NoSQL database.
- **xUnit**: Used for unit tests (and integration tests if needed).

## Prerequisites

- .NET 8 SDK
- Azure Cosmos DB Account or Azure Cosmos DB Emulator

## Installation and Setup

1. **Clone the repository to your local machine:**

    ```bash
    git clone <repository-url>
    ```

2. **Open the solution in your IDE.**

3. **Configure Azure Cosmos DB:**

    - If you are using the Azure Cosmos DB Emulator locally, ensure it's installed and running on your machine. You can find the emulator installation instructions [here](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator).
    - Obtain the following details from your Azure Cosmos DB account or emulator: `EndpointUri`, `DatabaseName`, and `PrimaryKey`.
    - Add these details to the `appsettings.json` file under the `CosmosDb` section:

    ```json
    {
      "CosmosDb": {
        "ServiceUri": "https://localhost:8081",
        "Key": "your-primary-key",
        "DatabaseName": "your-database-name",
        "EmployerContainerName": "your-employer-container-name",
        "EmployeeContainerName": "your-employee-container-name",
        "EmployeePersonalInfoContainerName": "your-employee-personal-info-container-name"
      }
    }
    ```

## Running Tests

Unit tests for this project are written using xUnit. To run the tests, use the following command in the terminal:

```bash
dotnet test

