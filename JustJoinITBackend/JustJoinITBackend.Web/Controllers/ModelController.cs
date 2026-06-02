using JustJoinITBackend.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace JustJoinITBackend.Web.Controllers;

[ApiController]
[Route("models")]
public class ModelController(ModelService modelService) : Controller
{
    [HttpGet]
    public ActionResult<List<ModelDto>> GetAll()
    {
        var models = modelService.GetAllModels();

        return Ok(models);
    }
}
