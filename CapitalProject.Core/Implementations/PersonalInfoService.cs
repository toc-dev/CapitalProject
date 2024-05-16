using CapitalProject.Core.Interfaces;
using CapitalProject.Data.DTOs;
using CapitalProject.Data.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalProject.Core.Implementations
{
    public class PersonalInfoService: IPersonalInfoService
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;
        private readonly ILogger<PersonalInfoService>? _logger;

        public PersonalInfoService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer(databaseName, containerName);
        }
        public PersonalInfoService(CosmosClient cosmosClient, string databaseName, string containerName, ILogger<PersonalInfoService> logger)
        {
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer(databaseName, containerName);
            _logger = logger;
        }
        public async Task<PersonalInformationDisplayDTO> ProvidePersonalInformation(PersonalInformationDTO model)
        {
            try
            {
                var personalInfo = new PersonalInformation()
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Nationality = model.Nationality,
                    CurrentResidence = model.CurrentResidence,
                    IDNumber = model.IDNumber,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                };
                await _container.CreateItemAsync(personalInfo, new PartitionKey(personalInfo.Id));
                return new PersonalInformationDisplayDTO()
                {
                    FirstName = personalInfo.FirstName,
                    LastName = personalInfo.LastName,
                    Email = personalInfo.Email,
                    Phone = personalInfo.Phone,
                    Nationality = personalInfo.Nationality,
                    CurrentResidence = personalInfo.CurrentResidence,
                    IDNumber = personalInfo.IDNumber,
                    DateOfBirth = personalInfo.DateOfBirth,
                    Gender = personalInfo.Gender,
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                throw;
            }
        }
    }
}
