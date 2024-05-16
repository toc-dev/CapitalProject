using CapitalProject.Core.Implementations;
using CapitalProject.Data.DTOs;
using CapitalProject.Data.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static CapitalProject.Data.Enums.Enumerations;

namespace CapitalProject.Tests
{
    public class EmployeeServiceTests
    {
        private readonly Mock<CosmosClient> _mockCosmosClient;
        private readonly Mock<Container> _mockContainer;
        private readonly Mock<ILogger<EmployeeService>> _mockLogger;
        private readonly EmployeeService _employeeService;

        public EmployeeServiceTests()
        {
            _mockCosmosClient = new Mock<CosmosClient>();
            _mockContainer = new Mock<Container>();
            _mockLogger = new Mock<ILogger<EmployeeService>>();

            _mockCosmosClient.Setup(x => x.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(_mockContainer.Object);

            _employeeService = new EmployeeService(_mockCosmosClient.Object, "databaseName", "containerName", _mockLogger.Object);
        }

        [Fact]
        public async Task AnswerQuestion_ShouldUpdateAnswer()
        {
            // Arrange
            var id = "questionId";
            var model = new AnswerQuestionDTO { Answer = "Updated Answer" };
            var customQuestion = new CustomQuestion { Id = id, QuestionType = QuestionType.Paragraph, Question = "Sample Question" };

            var readResponse = new Mock<ItemResponse<CustomQuestion>>();
            readResponse.Setup(r => r.Resource).Returns(customQuestion);

            _mockContainer.Setup(x => x.ReadItemAsync<CustomQuestion>(id, It.IsAny<PartitionKey>(), null, default))
                          .ReturnsAsync(readResponse.Object);
            _mockContainer.Setup(x => x.ReplaceItemAsync(It.IsAny<CustomQuestion>(), It.IsAny<string>(), It.IsAny<PartitionKey>(), null, default))
                          .ReturnsAsync(readResponse.Object);

            // Act
            var result = await _employeeService.AnswerQuestion(id, model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.Answer, customQuestion.Answer);
            _mockLogger.Verify(x => x.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetAllQuestions_ShouldReturnAllQuestions()
        {
            // Arrange
            var customQuestions = new List<CustomQuestion>
            {
                new CustomQuestion { Id = "1", QuestionType = QuestionType.MultipleChoice, Question = "Question1" },
                new CustomQuestion { Id = "2", QuestionType = QuestionType.Paragraph, Question = "Question2" }
            };

            var mockFeedIterator = new Mock<FeedIterator<CustomQuestion>>();
            mockFeedIterator.SetupSequence(x => x.HasMoreResults)
                            .Returns(true)
                            .Returns(false);
            mockFeedIterator.Setup(x => x.ReadNextAsync(default))
                            .ReturnsAsync(MockFeedResponse(customQuestions));

            _mockContainer.Setup(x => x.GetItemQueryIterator<CustomQuestion>(It.IsAny<QueryDefinition>(), null, null))
                          .Returns(mockFeedIterator.Object);

            // Act
            var result = await _employeeService.GetAllQuestions();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetQuestion_ShouldReturnQuestionById()
        {
            // Arrange
            var id = "questionId";
            var customQuestion = new CustomQuestion { Id = id, QuestionType = QuestionType.Paragraph, Question = "Sample Question" };

            var readResponse = new Mock<ItemResponse<CustomQuestion>>();
            readResponse.Setup(r => r.Resource).Returns(customQuestion);

            _mockContainer.Setup(x => x.ReadItemAsync<CustomQuestion>(id, It.IsAny<PartitionKey>(), null, default))
                          .ReturnsAsync(readResponse.Object);

            // Act
            var result = await _employeeService.GetQuestion(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customQuestion.Question, result.Question);
        }

        private static FeedResponse<CustomQuestion> MockFeedResponse(List<CustomQuestion> customQuestions)
        {
            var mockFeedResponse = new Mock<FeedResponse<CustomQuestion>>();
            mockFeedResponse.Setup(x => x.GetEnumerator()).Returns(customQuestions.GetEnumerator());
            mockFeedResponse.Setup(x => x.Resource).Returns(customQuestions);
            return mockFeedResponse.Object;
        }
    }
}
