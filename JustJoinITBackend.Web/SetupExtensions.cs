using JustJoinITBackend.Common;
using JustJoinITBackend.Web.Services;

namespace JustJoinITBackend.Web;

static public class SetupExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<JustJoinITBackendDbContext>();

        builder.Services.AddScoped<PromptService>();
    }

    public static void CreateDatabase(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<JustJoinITBackendDbContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}
