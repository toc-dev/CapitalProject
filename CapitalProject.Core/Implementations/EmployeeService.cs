﻿using CapitalProject.Core.Interfaces;
using CapitalProject.Data.DTOs;
using CapitalProject.Data.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly ILogger<EmployeeService>? _logger;

        public EmployeeService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer(databaseName, containerName);
        }
        public EmployeeService(CosmosClient cosmosClient, string databaseName, string containerName, ILogger<EmployeeService> logger)
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
            var query = _container.GetItemLinqQueryable<CustomQuestion>().Where(x=>x.QuestionType==questionType).ToFeedIterator();
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

    }
}
