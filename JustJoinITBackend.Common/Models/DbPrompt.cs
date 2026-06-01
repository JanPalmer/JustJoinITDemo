namespace JustJoinITBackend.Common.Models;

public class DbPrompt
{
    public int Id { get; set; }
    public int ModelId { get; set; }
    public DbModel Model { get; set; } = null!;

    public string Content { get; set; } = string.Empty;
    public DbPromptStatus Status { get; set; }

    public string? Result { get; set; }
    public string? ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
