using CapitalProject.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CapitalProject.Data.Enums.Enumerations;

namespace CapitalProject.Core.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<DisplayCustomQuestionsCandidate>> GetAllQuestions();
        Task<DisplayCustomQuestionsCandidate> GetQuestion(string id);
        public Task<List<DisplayCustomQuestionsCandidate>> GetQuestionByType(QuestionType questionType);
        Task<DisplayCustomQuestionsCandidate> AnswerQuestion(string id, AnswerQuestionDTO model);
    }
}
