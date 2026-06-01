using JustJoinITBackend.Common.Models;
using JustJoinITBackend.Web.Infrastructure;

namespace JustJoinITBackend.Web.Models;

public class PromptDto
{
    public int Id { get; set; }
    public string ModelName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public PromptStatus Status { get; set; }

    public string? Result { get; set; }
    public string? ErrorMessage { get; set; }

    static public PromptDto FromDbPrompt(DbPrompt dbPrompt)
    {
        return new PromptDto
        {
            Id = dbPrompt.Id,
            ModelName = dbPrompt.Model.Name,
            Content = dbPrompt.Content,
            Status = PromptStatusConverter.ConvertPromptStatus(dbPrompt.Status),
            Result = dbPrompt.Result,
            ErrorMessage = dbPrompt.ErrorMessage
        };
    }
}
