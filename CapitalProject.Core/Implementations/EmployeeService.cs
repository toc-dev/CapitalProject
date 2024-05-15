using CapitalProject.Core.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalProject.Core.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;
        private readonly ILogger<EmployerService>? _logger;

        public EmployeeService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer(databaseName, containerName);
        }
        public EmployeeService(CosmosClient cosmosClient, string databaseName, string containerName, ILogger<EmployerService> logger)
        {
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer(databaseName, containerName);
            _logger = logger;
        }
        public Task<DisplayCustomQuestionDTO> AnswerQuestion(AnswerQuestionDTO model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DisplayCustomQuestionsCandidate>> GetAllQuestions()
        {
            var query = _container.GetItemQueryIterator<CustomQuestion>(new QueryDefinition("SELECT * FROM c"));
            List<CustomQuestion> results = new List<CustomQuestion>();
            var result = new DisplayCustomQuestionsCandidate();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            var allQuestions = results.Select(MapToDto).ToList();
            return allQuestions;
        }

        public async Task<DisplayCustomQuestionsCandidate> GetQuestion(string id)
        {
            var query = await _container.ReadItemAsync<CustomQuestion>(id, new PartitionKey(id));

            return MapToDto(query);
        }
        private static DisplayCustomQuestionsCandidate MapToDto(CustomQuestion entity)
        {
            var question = new DisplayCustomQuestionsCandidate();
            switch (entity.QuestionType)
            {
                case QuestionType.Paragraph:
                    question.QuestionType = entity.QuestionType;
                    question.Question = entity.Question;
                    question.Answer = entity.ParagraphAnswer;
                    break;
                case QuestionType.YesorNo:
                    question.QuestionType = entity.QuestionType;
                    question.Question = entity.Question;
                    question.Answer = entity.ParagraphAnswer;
                    break;
                case QuestionType.Dropdown:
                    question.QuestionType = entity.QuestionType;
                    question.Question = entity.Question;
                    question.Answer = entity.ParagraphAnswer;
                    break;
                case QuestionType.MultipleChoice:
                    question.QuestionType = entity.QuestionType;
                    question.Question = entity.Question;
                    question.Answer = entity.ParagraphAnswer;
                    break;
                case QuestionType.Date:
                    question.QuestionType = entity.QuestionType;
                    question.Question = entity.Question;
                    question.Answer = entity.ParagraphAnswer;
                    break;
                case QuestionType.Number:
                    question.QuestionType = entity.QuestionType;
                    question.Question = entity.Question;
                    question.Answer = entity.ParagraphAnswer;
                    break;
                default:
                    break;

            }
            return question;
        }
    }
}
