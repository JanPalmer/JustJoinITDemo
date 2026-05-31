using JustJoinITBackend.Common;
using JustJoinITBackend.Common.Models;
using JustJoinITBackend.Web.Models;

namespace JustJoinITBackend.Web.Services;

public class PromptService(JustJoinITBackendDbContext dbContext)
{
    public List<PromptDto> GetAllPrompts()
    {
        return dbContext.Prompts.Select(p => PromptDto.FromDbPrompt(p)).ToList();
    }

    public PromptDto? GetPrompt(int id)
    {
        var prompt = dbContext.Prompts.FirstOrDefault(x => x.Id == id);

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
            Content = request.Content,
            Status = DbPromptStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        var addedPrompt = await dbContext.Prompts.AddAsync(newPrompt);
        await dbContext.SaveChangesAsync();

        var result = PromptDto.FromDbPrompt(addedPrompt.Entity);

        return result;
    }
}
