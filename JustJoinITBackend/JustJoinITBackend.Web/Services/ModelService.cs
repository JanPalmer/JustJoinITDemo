using JustJoinITBackend.Common;
using Microsoft.EntityFrameworkCore;

namespace JustJoinITBackend.Web.Services;

public class ModelService(JustJoinITBackendDbContext dbContext)
{
    public List<ModelDto> GetAllModels()
    {
        return dbContext.Models
            .AsNoTracking()
            .Select(ModelDto.FromDbModel)
            .ToList();
    }
}
