using CapitalProject.Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CapitalProject.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        private readonly IEmployerService _employerService;
        //private readonly ILogger<EmployerController> _logger;
        public EmployerController(IEmployerService employerService)
        {
            _employerService = employerService;
        }

        [HttpPost("(createquestion)")]
        [ProducesResponseType(200, Type = typeof(DisplayCustomQuestionDTO))]
        [ProducesResponseType(400, Type = typeof(List<string>))]
        public async Task<IActionResult> CreateCustomQuestion(CreateCustomQuestionDTO model)
        {
            try
            {
                var response = await _employerService.CreateCustomQuestion(model);
                return Ok(new CapitalCustomAPIResponseSchema("Custom Question Creation Successful", response));
            }
            catch (Exception ex)
            {
                return BadRequest(new CapitalCustomAPIResponseSchema(new List<string>() { ex.Message }));
            }
        }
        [HttpPost("(updatequestion)")]
        [ProducesResponseType(200, Type = typeof(DisplayCustomQuestionDTO))]
        [ProducesResponseType(400, Type = typeof(List<string>))]
        public async Task<IActionResult> UpdateCustomQuestion(string id, [FromBody] UpdateCustomQuestionDTO model)
        {
            try
            {
                var response = await _employerService.UpdateCustomQuestion(id, model);
                return Ok(new CapitalCustomAPIResponseSchema("Custom Question Update Successful", response));
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
                await _employerService.DeleteCustomQuestion(id);
                return Ok(new CapitalCustomAPIResponseSchema("Custom Question Deleted"));
            }
            catch (Exception ex)
            {
                return BadRequest(new CapitalCustomAPIResponseSchema(new List<string>() { ex.Message }));
            }
        }
    }
}
