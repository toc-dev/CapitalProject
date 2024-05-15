using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CapitalProject.Data.Enums.Enumerations;

namespace CapitalProject.Data.DTOs
{
    public class CreateCustomQuestionDTO
    {
        public QuestionType QuestionType { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public MultipleChoiceAnswerDTO? MultipleChoiceAnswer { get; set; }

    }
    public class MultipleChoiceAnswerDTO
    {
        public List<string>? Choices { get; set; }
        public int? MaxChoiceAllowed { get; set; }
        public string? Answer { get;}
    }

    public class UpdateCustomQuestionDTO: CreateCustomQuestionDTO
    {
    }

    public class AnswerQuestionDTO
    {
        public string? Answer { get; set; }
        public List<string>? MultipleChoiceAnswer { get; set; }
    }
}
