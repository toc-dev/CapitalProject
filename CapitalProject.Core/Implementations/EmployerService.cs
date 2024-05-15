using CapitalProject.Core.Interfaces;
using CapitalProject.Data.DTOs;
using CapitalProject.Data.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CapitalProject.Data.Enums.Enumerations;

namespace CapitalProject.Core.Implementations
{
    public class EmployerService : IEmployerService
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;
        private readonly ILogger<EmployerService>? _logger;

        public EmployerService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer(databaseName, containerName);
        }
        public EmployerService(CosmosClient cosmosClient, string databaseName, string containerName, ILogger<EmployerService> logger)
        {
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer(databaseName, containerName);
            _logger = logger;
        }
        public async Task<DisplayCustomQuestionDTO> CreateCustomQuestion(CreateCustomQuestionDTO model)
        {
            try
            {
                var question = new CustomQuestion();

                switch (model.QuestionType)
                {
                    case QuestionType.MultipleChoice:
                        question.Id = Guid.NewGuid().ToString();
                        question.QuestionType = model.QuestionType;
                        question.Question = model.Question;
                        question.MultipleChoiceAnswer = new MultipleChoiceAnswer()
                        {
                            MaxChoiceAllowed = model?.MultipleChoiceAnswer?.MaxChoiceAllowed,
                            Choices = model?.MultipleChoiceAnswer?.Choices,
                        };
                        break;
                    default:
                        question.Id = Guid.NewGuid().ToString();
                        question.QuestionType = model.QuestionType;
                        question.Question = model.Question;
                        break;
                }
                await _container.CreateItemAsync(question, new PartitionKey(question.Id));
                return new DisplayCustomQuestionDTO()
                {
                    QuestionType = question.QuestionType,
                    Question = question.Question
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError($"{ex.Message}", ex);
                throw;
            }
        }
        public async Task<DisplayCustomQuestionDTO> UpdateCustomQuestion(string id, UpdateCustomQuestionDTO model)
        {
            try
            {

                var question = await _container.ReadItemAsync<CustomQuestion>(id, new PartitionKey(id));
                var item = question.Resource;
                item.Question = model.Question;
                item.QuestionType = model.QuestionType;


                switch (model.QuestionType)
                {
                    case QuestionType.MultipleChoice:
                        item.QuestionType = model.QuestionType;
                        item.Question = model.Question;
                        item.MultipleChoiceAnswer = new MultipleChoiceAnswer()
                        {
                            MaxChoiceAllowed = model?.MultipleChoiceAnswer?.MaxChoiceAllowed,
                            Choices = model?.MultipleChoiceAnswer?.Choices,
                        };
                        break;
                    default:
                        item.QuestionType = model.QuestionType;
                        item.Question = model.Question;
                        break;
                }
                await _container.ReplaceItemAsync(item, id, new PartitionKey(id));
                _logger?.LogInformation($"Updated custom question: {model.Question}");

                return new DisplayCustomQuestionDTO()
                {
                    QuestionType = question.Resource.QuestionType,
                    Question = question.Resource.Question
                };

            }
            catch (Exception ex)
            {
                _logger?.LogError($"{ex.Message}", ex);
                throw;
            }
        }
        public async Task DeleteCustomQuestion(string id)
        {
            try
            {
                await _container.DeleteItemAsync<CustomQuestion>(id, new PartitionKey(id));
                _logger?.LogInformation($"Question with {id} deleted");
            }
            catch (Exception ex)
            {
                _logger?.LogError($"{ex.Message}", ex);
                throw;
            }
        }
    }
}
