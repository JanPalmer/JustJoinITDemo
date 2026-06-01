using JustJoinITBackend.Web.Models;

namespace JustJoinITBackend.Web.Infrastructure;

public static class PromptStatusConverter
{
    private readonly static Dictionary<Common.Models.DbPromptStatus, PromptStatus> _dbToDtoStatus = new()
    {
        { Common.Models.DbPromptStatus.Unknown, PromptStatus.Unknown },
        { Common.Models.DbPromptStatus.Pending, PromptStatus.Pending },
        { Common.Models.DbPromptStatus.Processing, PromptStatus.Processing },
        { Common.Models.DbPromptStatus.Completed, PromptStatus.Completed },
        { Common.Models.DbPromptStatus.Failed, PromptStatus.Failed },
    };

    private readonly static Dictionary<PromptStatus, Common.Models.DbPromptStatus> _dtoToDbStatus =
        _dbToDtoStatus.ToDictionary(kv => kv.Value, kv => kv.Key);

    public static PromptStatus ConvertPromptStatus(Common.Models.DbPromptStatus dbPromptStatus)
    {
        if (_dbToDtoStatus.TryGetValue(dbPromptStatus, out var dtoPromptStatus))
        {
            return dtoPromptStatus;
        }

        throw new ArgumentException($"Unknown DbPromptStatus: {dbPromptStatus.ToString()}");
    }

    public static Common.Models.DbPromptStatus ConvertPromptStatus(PromptStatus dtoPromptStatus)
    {
        if (_dtoToDbStatus.TryGetValue(dtoPromptStatus, out var dbPromptStatus))
        {
            return dbPromptStatus;
        }

        throw new ArgumentException($"Unknown PromptStatus: {dtoPromptStatus.ToString()}");
    }
}
