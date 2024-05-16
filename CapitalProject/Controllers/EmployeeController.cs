using CapitalProject.Core.Interfaces;
using CapitalProject.Core.Utilities;
using CapitalProject.Data.DTOs;
using Microsoft.AspNetCore.Mvc;
using static CapitalProject.Data.Enums.Enumerations;

namespace CapitalProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IPersonalInfoService _personalInfoService;
        //private readonly ILogger<EmployerController> _logger;
        public EmployeeController(IEmployeeService employeeService, IPersonalInfoService personalInfoService)
        {
            _employeeService = employeeService;
            _personalInfoService = personalInfoService;
        }

        [HttpGet("(getallquestions)")]
        [ProducesResponseType(200, Type = typeof(DisplayCustomQuestionDTO))]
        [ProducesResponseType(400, Type = typeof(List<string>))]
        public async Task<IActionResult> GetAllQuestions()
        {
            try
            {
                var response = await _employeeService.GetAllQuestions();
                return Ok(new CapitalCustomAPIResponseSchema("Question Retrieval Successful", response));
            }
            catch (Exception ex)
            {
                return BadRequest(new CapitalCustomAPIResponseSchema(new List<string>() { ex.Message }));
            }
        }
        [HttpGet("(getquestionsbytype)")]
        [ProducesResponseType(200, Type = typeof(DisplayCustomQuestionDTO))]
        [ProducesResponseType(400, Type = typeof(List<string>))]
        public async Task<IActionResult> GetQuestionsByType(QuestionType questionType)
        {
            try
            {
                var response = await _employeeService.GetQuestionByType(questionType);
                return Ok(new CapitalCustomAPIResponseSchema("Question Retrieval Successful", response));
            }
            catch (Exception ex)
            {
                return BadRequest(new CapitalCustomAPIResponseSchema(new List<string>() { ex.Message }));
            }
        }
        [HttpGet("(getquestion)")]
        [ProducesResponseType(200, Type = typeof(DisplayCustomQuestionsCandidate))]
        [ProducesResponseType(400, Type = typeof(List<string>))]
        public async Task<IActionResult> GetQuestion(string id)
        {
            try
            {
                var response = await _employeeService.GetQuestion(id);
                return Ok(new CapitalCustomAPIResponseSchema("Custom Question retrieved", response));
            }
            catch (Exception ex)
            {
                return BadRequest(new CapitalCustomAPIResponseSchema(new List<string>() { ex.Message }));
            }
        }
        [HttpPost("(answerquestion)")]
        [ProducesResponseType(200, Type = typeof(DisplayCustomQuestionDTO))]
        [ProducesResponseType(400, Type = typeof(List<string>))]
        public async Task<IActionResult> UpdateCustomQuestion(string id, [FromBody] AnswerQuestionDTO model)
        {
            try
            {
                var response = await _employeeService.AnswerQuestion(id, model);
                return Ok(new CapitalCustomAPIResponseSchema("Custom Question Answer Successful", response));
            }
            catch (Exception ex)
            {
                return BadRequest(new CapitalCustomAPIResponseSchema(new List<string>() { ex.Message }));
            }
        }
        
        [HttpPost("(personalinfo)")]
        [ProducesResponseType(200, Type = typeof(DisplayCustomQuestionDTO))]
        [ProducesResponseType(400, Type = typeof(List<string>))]
        public async Task<IActionResult> ProvidePersonalInformation([FromBody] PersonalInformationDTO model)
        {
            try
            {
                var response = await _personalInfoService.ProvidePersonalInformation(model);
                return Ok(new CapitalCustomAPIResponseSchema("Information Recorded", response));
            }
            catch (Exception ex)
            {
                return BadRequest(new CapitalCustomAPIResponseSchema(new List<string>() { ex.Message }));
            }
        }
    }
}
