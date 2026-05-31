using JustJoinITBackend.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace JustJoinITBackend.Common;

public class JustJoinITBackendDbContext : DbContext
{
    // The database is created through SetupExtensions, with the model being taken from the rules defined in this file.
    // This was done to simplify implementation by not creating separate create/upgrade scripts for the task.

    private const string DbName = "JustJoinITBackend.db";
    public string DbPath { get; set; } = string.Empty;

    public JustJoinITBackendDbContext(DbContextOptions<JustJoinITBackendDbContext> options) : base(options)
    {
        // A bit hacky, but because we create the database locally and want to access it from both the Backend and the Worker,
        // it's easiest to create the DB file in the Solution directory, so the folder is easily found from both projects.
        // Normally, both projects would have their own connection strings to the database, instead of putting the path here.
        var value = Assembly
            .GetExecutingAssembly()
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(a => a.Key == "SolutionDir")
            ?.Value;
        DbPath = System.IO.Path.Join(value, DbName);

        //var folder = Directory.GetParent(Environment.CurrentDirectory);
        //var path = folder.FullName;
        //DbPath = System.IO.Path.Join(path, "JustJoinITBackend.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}", c => c.CommandTimeout(10));

    public virtual DbSet<DbPrompt> Prompts => Set<DbPrompt>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbPrompt>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id)
                    .ValueGeneratedOnAdd();
            entity.Property(p => p.Content)
                    .IsRequired();
            entity.Property(e => e.Result)
                    .IsRequired(false);
            entity.Property(e => e.ErrorMessage)
                    .IsRequired(false);
        });
    }
}
