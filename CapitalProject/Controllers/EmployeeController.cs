using CapitalProject.Core.Interfaces;
using CapitalProject.Core.Utilities;
using CapitalProject.Data.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CapitalProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        //private readonly ILogger<EmployerController> _logger;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("(createquestion)")]
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
        [HttpPost("(deletequestion)")]
        [ProducesResponseType(200, Type = typeof(DisplayCustomQuestionDTO))]
        [ProducesResponseType(400, Type = typeof(List<string>))]
        public async Task<IActionResult> DeleteCustomQuestion(string id)
        {
            try
            {
                await _employeeService.GetQuestion(id);
                return Ok(new CapitalCustomAPIResponseSchema("Custom Question Deleted"));
            }
            catch (Exception ex)
            {
                return BadRequest(new CapitalCustomAPIResponseSchema(new List<string>() { ex.Message }));
            }
        }
        [HttpPost("(updatequestion)")]
        [ProducesResponseType(200, Type = typeof(DisplayCustomQuestionDTO))]
        [ProducesResponseType(400, Type = typeof(List<string>))]
        public async Task<IActionResult> UpdateCustomQuestion(string id, [FromBody] AnswerQuestionDTO model)
        {
            try
            {
                var response = await _employeeService.AnswerQuestion(id, model);
                return Ok(new CapitalCustomAPIResponseSchema("Custom Question Update Successful", response));
            }
            catch (Exception ex)
            {
                return BadRequest(new CapitalCustomAPIResponseSchema(new List<string>() { ex.Message }));
            }
        }
    }
}
