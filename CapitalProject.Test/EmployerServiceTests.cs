using CapitalProject.Core.Implementations;
using CapitalProject.Data.DTOs;
using CapitalProject.Data.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using static CapitalProject.Data.Enums.Enumerations;

namespace CapitalProject.Test
{
    public class EmployerServiceTests
    {
        private readonly Mock<CosmosClient> _mockCosmosClient;
        private readonly Mock<Container> _mockContainer;
        private readonly Mock<ILogger<EmployerService>> _mockLogger;
        private readonly EmployerService _employerService;

        public EmployerServiceTests()
        {
            _mockCosmosClient = new Mock<CosmosClient>();
            _mockContainer = new Mock<Container>();
            _mockLogger = new Mock<ILogger<EmployerService>>();

            _mockCosmosClient.Setup(x => x.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(_mockContainer.Object);

            _employerService = new EmployerService(_mockCosmosClient.Object, "databaseName", "containerName", _mockLogger.Object);
        }

        [Fact]
        public async Task CreateCustomQuestion_ShouldCreateQuestion()
        {
            // Arrange
            var model = new CreateCustomQuestionDTO
            {
                QuestionType = QuestionType.Paragraph,
                Question = "Sample Question"
            };
            var createdQuestion = new CustomQuestion
            {
                Id = Guid.NewGuid().ToString(),
                QuestionType = model.QuestionType,
                Question = model.Question
            };

            _mockContainer.Setup(x => x.CreateItemAsync(It.IsAny<CustomQuestion>(), It.IsAny<PartitionKey>(), null, default))
                          .ReturnsAsync(new Mock<ItemResponse<CustomQuestion>>().Object);

            // Act
            var result = await _employerService.CreateCustomQuestion(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.QuestionType, result.QuestionType);
            Assert.Equal(model.Question, result.Question);
        }

        [Fact]
        public async Task UpdateCustomQuestion_ShouldUpdateQuestion()
        {
            // Arrange
            var id = "questionId";
            var model = new UpdateCustomQuestionDTO
            {
                QuestionType = QuestionType.Paragraph,
                Question = "Updated Question"
            };
            var customQuestion = new CustomQuestion { Id = id, QuestionType = QuestionType.Paragraph, Question = "Sample Question" };

            var readResponse = new Mock<ItemResponse<CustomQuestion>>();
            readResponse.Setup(r => r.Resource).Returns(customQuestion);

            _mockContainer.Setup(x => x.ReadItemAsync<CustomQuestion>(id, It.IsAny<PartitionKey>(), null, default))
                          .ReturnsAsync(readResponse.Object);
            _mockContainer.Setup(x => x.ReplaceItemAsync(It.IsAny<CustomQuestion>(), id, It.IsAny<PartitionKey>(), null, default))
                          .ReturnsAsync(readResponse.Object);

            // Act
            var result = await _employerService.UpdateCustomQuestion(id, model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.Question, result.Question);
            Assert.Equal(model.QuestionType, result.QuestionType);
            _mockLogger.Verify(x => x.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomQuestion_ShouldDeleteQuestion()
        {
            // Arrange
            var id = "questionId";

            _mockContainer.Setup(x => x.DeleteItemAsync<CustomQuestion>(id, It.IsAny<PartitionKey>(), null, default))
                          .ReturnsAsync(new Mock<ItemResponse<CustomQuestion>>().Object);

            // Act
            await _employerService.DeleteCustomQuestion(id);

            // Assert
            _mockContainer.Verify(x => x.DeleteItemAsync<CustomQuestion>(id, It.IsAny<PartitionKey>(), null, default), Times.Once);
            _mockLogger.Verify(x => x.LogInformation(It.IsAny<string>()), Times.Once);
        }
    }
}
