using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CapitalProject.Data.Enums.Enumerations;

namespace CapitalProject.Data.Entities
{
    public class CustomQuestion
    {
        [JsonProperty(PropertyName = "id")]
        public string? Id { get; set; }
        public QuestionType QuestionType { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public MultipleChoiceAnswer? MultipleChoiceAnswer { get; set; }
    }
    public class MultipleChoiceAnswer
    {
        public List<string>? Choices { get; set; }
        public int? MaxChoiceAllowed { get; set; }
        public List<string>? Answers { get; set; }
    }
}
