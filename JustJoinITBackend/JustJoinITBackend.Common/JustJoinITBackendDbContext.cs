using JustJoinITBackend.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace JustJoinITBackend.Common;

public class JustJoinITBackendDbContext : DbContext
{
    // The database is created in SetupExtensions.cs, with the model being taken from the rules defined in this file, in OnModelCreating().
    // This was done to simplify implementation by not creating separate create/upgrade scripts for the task.

    private const string DbName = "JustJoinITBackend.db";
    public string DbPath { get; set; } = string.Empty;

    public JustJoinITBackendDbContext(DbContextOptions<JustJoinITBackendDbContext> options) : base(options)
    {
        // A bit hacky, but because we create the database locally and want to access it from both the Backend and the Worker,
        // it's easiest to create the DB file in the Solution directory, so the folder is easily found from both projects.
        // Normally, both projects would have their own DB connection strings and connection configuration on startup, instead of putting it here
        // and setting it up through OnConfiguring().
        var value = Assembly
            .GetExecutingAssembly()
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(a => a.Key == "SolutionDir")
            ?.Value;
        DbPath = System.IO.Path.Join(value, DbName);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}", c => c.CommandTimeout(10));

    public virtual DbSet<DbModel> Models => Set<DbModel>();
    public virtual DbSet<DbPrompt> Prompts => Set<DbPrompt>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbModel>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id)
                    .ValueGeneratedOnAdd();
            entity.Property(m => m.Name)
                    .IsRequired();
            entity.HasData(
                new DbModel { Id = 1, Name = "fake-model", Family = "FakeModel" },
                new DbModel { Id = 2, Name = "llama-3.3-70b-versatile", Family = "Groq" },
                new DbModel { Id = 3, Name = "gemini-3.5-flash", Family = "Gemini" },
                new DbModel { Id = 4, Name = "gemini-3.1-flash-lite", Family = "Gemini" }
            );
        });

        modelBuilder.Entity<DbPrompt>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasOne(p => p.Model)
                    .WithMany()
                    .HasForeignKey(p => p.ModelId)
                    .OnDelete(DeleteBehavior.Cascade);
            entity.Property(p => p.Status)
                    .IsRequired();
            entity.Property(p => p.Content)
                    .IsRequired();
            entity.Property(e => e.Result)
                    .IsRequired(false);
            entity.Property(e => e.ErrorMessage)
                    .IsRequired(false);
        });
    }
}
