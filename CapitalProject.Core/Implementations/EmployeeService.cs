using CapitalProject.Core.Interfaces;
using CapitalProject.Data.DTOs;
using CapitalProject.Data.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static CapitalProject.Data.Enums.Enumerations;

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
        public async Task<DisplayCustomQuestionsCandidate> AnswerQuestion(string id, AnswerQuestionDTO model)
        {
            try
            {

                var question = await _container.ReadItemAsync<CustomQuestion>(id, new PartitionKey(id));
                var item = question.Resource;

                switch (item.QuestionType)
                {
                    case QuestionType.MultipleChoice:
                        item.Answer = model.Answer;
                        item.MultipleChoiceAnswer = new MultipleChoiceAnswer()
                        {
                            Answers = model?.MultipleChoiceAnswer,
                        };
                        _logger?.LogInformation($"Answered custom question: {model?.MultipleChoiceAnswer}");
                        break;
                    default:
                        item.Answer = model.Answer;
                        _logger?.LogInformation($"Answered custom question: {model?.Answer}");
                        break;
                }
                await _container.ReplaceItemAsync(item, id, new PartitionKey(id));
                

                return new DisplayCustomQuestionsCandidate()
                {
                    QuestionType = question.Resource.QuestionType,
                    Question = question.Resource.Question,
                    Answer = model?.Answer,
                };

            }
            catch (Exception ex)
            {
                _logger?.LogError($"{ex.Message}", ex);
                throw;
            }
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

            var allQuestions = results.Select(MapAnswerToDto).ToList();
            return allQuestions;
        }
         public async Task<List<DisplayCustomQuestionsCandidate>> GetQuestionByType(QuestionType questionType)
        {
            var query = _container.GetItemQueryIterator<CustomQuestion>(new QueryDefinition($"SELECT * FROM c WHERE c.QuestionType = @questionType").WithParameter("@questionType", questionType.ToString()));
            List<CustomQuestion> results = new List<CustomQuestion>();
            var result = new DisplayCustomQuestionsCandidate();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            var allQuestions = results.Select(MapAnswerToDto).ToList();
            return allQuestions;
        }

        public async Task<DisplayCustomQuestionsCandidate> GetQuestion(string id)
        {
            var query = await _container.ReadItemAsync<CustomQuestion>(id, new PartitionKey(id));

            return MapAnswerToDto(query);
        }
        private static DisplayCustomQuestionsCandidate MapAnswerToDto(CustomQuestion entity)
        {
            var question = new DisplayCustomQuestionsCandidate();
            switch (entity.QuestionType)
            {
                case QuestionType.MultipleChoice:
                    question.QuestionType = entity.QuestionType;
                    question.Question = entity.Question;
                    question.MultipleChoiceAnswer = entity?.MultipleChoiceAnswer?.Answers;
                    break;
                default:
                    question.QuestionType = entity.QuestionType;
                    question.Question = entity.Question;
                    question.Answer = entity.Answer;
                    break;
            }
            return question;
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
