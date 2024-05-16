# CapitalProject
This is a .NET 8 application that leverages Azure Cosmos Db to store and retrieve custom questions created by users
Below, you will find the necessary configurations and dependencies.

#Technologies
.NET 8: Newer versions of .NET come with such features as implicit usings and a deprecated startup class. As well as very robust reflection properties.
Clean Architecture: The project remains faithful to the core ideas of clean architecture principles, while also bearing in mind the size of the project
Azure Cosmos DB: Provides us with a noSQL db.
xUnit: leveraged for unit tests (and integration tests if the need ever arises).
Prerequisites
.NET 8 SDK
Azure Cosmos DB Account or Azure Cosmos DB Emulator

#Installation and Setup
Clone the repository to your local machine:

git clone <repository-url>
Open the solution in your preferred IDE (Visual Studio, Visual Studio Code, etc.).

#Configure Azure Cosmos DB:

If you are using the Azure Cosmos DB Emulator locally, make sure it's installed and running on your machine. You can find the emulator installation instructions here.
Obtain the following details from your Azure Cosmos DB account or emulator: EndpointUri, DatabaseName, and PrimaryKey.
Add these details to the appsettings.json file under the "CosmosDbSettings" section:
 "CosmosDb": {
   "ServiceUri": "https://localhost:8081",
   "Key": "your primary key",
   "DatabaseName": "your database name",
   "ContainerName": "your container name",
   "ContainerName2": "other containers"
 }

#Running Tests
Unit tests for this project are written using xUnit. To run the tests, use the following command in the terminal:

dotnet test
This will execute all the unit tests in the project and provide the test results.

License
This project is licensed under the MIT License. Feel free to use, modify, and distribute the code as per the license terms.
