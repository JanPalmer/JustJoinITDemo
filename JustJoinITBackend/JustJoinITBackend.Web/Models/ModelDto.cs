using JustJoinITBackend.Common.Models;

public class ModelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Family { get; set; } = string.Empty;

    static public ModelDto FromDbModel(DbModel dbModel)
    {
        return new ModelDto
        {
            Id = dbModel.Id,
            Name = dbModel.Name,
            Family = dbModel.Family,
        };
    }
}