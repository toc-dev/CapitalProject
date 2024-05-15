using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalProject.Data.DTOs
{
    public class CreateCustomQuestionDTO
    {
        public QuestionType QuestionType { get; set; }
        public string? Question { get; set; }
        public string? ParagraphAnswer { get; set; }
        public bool? YesNoAnswer { get; set; }
        public string? DropDownAnswer { get; set; }
        public string? MultipleChoiceAnswer { get; set; }
        public DateTime? Date { get; set; }
        public int? NumericalAnswer { get; set; }
        public int MaxChoiceAllowed { get; set; }
    }
    public class UpdateCustomQuestionDTO
    {
        public QuestionType QuestionType { get; set; }
        public string? Question { get; set; }
    }

    public class AnswerQuestionDTO
    {
        public int Answer { get; set; }
        public string StringAnswer { get; set; }
    }
}
