using CapitalProject.Core.Implementations;
using CapitalProject.Core.Interfaces;
using Microsoft.Azure.Cosmos;

namespace CapitalProject.API
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEmployerService>(InitializeCosmosClientInstanceEmployerAsync(configuration.GetSection("CosmosDb")).GetAwaiter().GetResult());
            services.AddSingleton<IEmployeeService>(InitializeCosmosClientInstanceEmployeeAsync(configuration.GetSection("CosmosDb")).GetAwaiter().GetResult());
            services.AddLogging();
        }

        public static async Task<EmployerService> InitializeCosmosClientInstanceEmployerAsync(IConfiguration configuration)
        {
            var databaseName = configuration["DatabaseName"];
            var containerName = configuration["ContainerName"];
            var account = configuration["ServiceUri"];
            var key = configuration["Key"];
            ILogger<EmployerService> logger;

            var client = new CosmosClient(account, key);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            var employerService = new EmployerService(client, databaseName, containerName);

            return employerService;
        }
         public static async Task<EmployeeService> InitializeCosmosClientInstanceEmployeeAsync(IConfiguration configuration)
        {
            var databaseName = configuration["DatabaseName"];
            var containerName = configuration["ContainerName"];
            var account = configuration["ServiceUri"];
            var key = configuration["Key"];
            ILogger<EmployerService> logger;

            var client = new CosmosClient(account, key);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            var employeeService = new EmployeeService(client, databaseName, containerName);

            return employeeService;
        }

    }
}
