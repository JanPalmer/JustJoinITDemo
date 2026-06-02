using JustJoinITBackend.Common;
using JustJoinITBackend.Common.Models;
using JustJoinITBackend.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace JustJoinITBackend.Web.Services;

public class PromptService(JustJoinITBackendDbContext dbContext)
{
    public List<PromptDto> GetAllPrompts()
    {
        return FindPrompts()
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(PromptDto.FromDbPrompt)
            .ToList();
    }

    public PromptDto? GetPrompt(int id)
    {
        var prompt = FindPrompts()
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        if (prompt != null)
        {
            return PromptDto.FromDbPrompt(prompt);
        }

        return null;
    }

    public async Task<PromptDto> AddPrompt(AddPromptRequest request)
    {
        var newPrompt = new DbPrompt
        {
            ModelId = request.ModelId,
            Content = request.Content,
            Status = DbPromptStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        var addedPrompt = await dbContext.Prompts.AddAsync(newPrompt);
        await dbContext.SaveChangesAsync();
        await dbContext.Entry(newPrompt).Reference(p => p.Model).LoadAsync();

        var result = PromptDto.FromDbPrompt(newPrompt);

        return result;
    }

    private IQueryable<DbPrompt> FindPrompts()
    {
        return dbContext.Prompts
            .Include(x => x.Model);
    }
}
