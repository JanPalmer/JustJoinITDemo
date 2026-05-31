using JustJoinITBackend.Common.Models;

namespace JustJoinITBackend.Web.Models;

public class PromptDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public PromptStatus Status { get; set; }

    public string? Result { get; set; }
    public string? ErrorMessage { get; set; }

    static public PromptDto FromDbPrompt(DbPrompt dbPrompt)
    {
        return new PromptDto
        {
            Id = dbPrompt.Id,
            Content = dbPrompt.Content,
            Status = (PromptStatus)dbPrompt.Status,
            Result = dbPrompt.Result,
            ErrorMessage = dbPrompt.ErrorMessage
        };
    }
}
