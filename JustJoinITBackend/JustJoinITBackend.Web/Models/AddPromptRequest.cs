namespace JustJoinITBackend.Web.Models;

public class AddPromptRequest
{
    public int ModelId { get; set; }
    public string Content { get; set; } = string.Empty;
}
