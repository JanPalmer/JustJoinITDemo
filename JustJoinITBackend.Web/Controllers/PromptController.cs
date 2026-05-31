using JustJoinITBackend.Web.Models;
using JustJoinITBackend.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace JustJoinITBackend.Web.Controllers;

[ApiController]
[Route("prompts")]
public class PromptController(PromptService promptService) : ControllerBase
{
    [HttpGet]
    public ActionResult<List<PromptDto>> GetAll()
    {
        var prompts = promptService.GetAllPrompts();

        return Ok(prompts);
    }

    [Route("{promptId}")]
    [HttpGet]
    public ActionResult<PromptDto> GetPromptWithId(int promptId)
    {
        var prompt = promptService.GetPrompt(promptId);

        return prompt != null ? Ok(prompt) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<PromptDto>> AddPrompt([FromBody] AddPromptRequest request)
    {
        var result = await promptService.AddPrompt(request);

        return Ok(result);
    }
}
