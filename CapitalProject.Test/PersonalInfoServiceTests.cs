using CapitalProject.Core.Implementations;
using CapitalProject.Data.DTOs;
using CapitalProject.Data.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CapitalProject.Tests
{
    public class PersonalInfoServiceTests
    {
        private readonly Mock<CosmosClient> _mockCosmosClient;
        private readonly Mock<Container> _mockContainer;
        private readonly Mock<ILogger<PersonalInfoService>> _mockLogger;
        private readonly PersonalInfoService _personalInfoService;

        public PersonalInfoServiceTests()
        {
            _mockCosmosClient = new Mock<CosmosClient>();
            _mockContainer = new Mock<Container>();
            _mockLogger = new Mock<ILogger<PersonalInfoService>>();

            _mockCosmosClient.Setup(x => x.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(_mockContainer.Object);

            _personalInfoService = new PersonalInfoService(_mockCosmosClient.Object, "databaseName", "containerName", _mockLogger.Object);
        }

        [Fact]
        public async Task ProvidePersonalInformation_ShouldCreatePersonalInfo()
        {
            // Arrange
            var model = new PersonalInformationDTO
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "1234567890",
                Nationality = "American",
                CurrentResidence = "USA",
                IDNumber = "ID12345",
                DateOfBirth = new DateTime(1990, 1, 1).ToString(),
                Gender = "Male"
            };

            var personalInfo = new PersonalInformation
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
                Gender = model.Gender
            };

            _mockContainer.Setup(x => x.CreateItemAsync(It.IsAny<PersonalInformation>(), It.IsAny<PartitionKey>(), null, default))
                          .ReturnsAsync(new Mock<ItemResponse<PersonalInformation>>().Object);

            // Act
            var result = await _personalInfoService.ProvidePersonalInformation(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.FirstName, result.FirstName);
            Assert.Equal(model.LastName, result.LastName);
            Assert.Equal(model.Email, result.Email);
            Assert.Equal(model.Phone, result.Phone);
            Assert.Equal(model.Nationality, result.Nationality);
            Assert.Equal(model.CurrentResidence, result.CurrentResidence);
            Assert.Equal(model.IDNumber, result.IDNumber);
            Assert.Equal(model.DateOfBirth, result.DateOfBirth);
            Assert.Equal(model.Gender, result.Gender);
        }
    }
}
