using Memoraid.WebApi.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Memoraid.WebApi.Persistence;

public class MemoraidDbContext : DbContext
{
    public MemoraidDbContext(DbContextOptions<MemoraidDbContext> options) : base(options)
    {
    }

    public DbSet<Flashcard> Flashcards { get; set; }
    public DbSet<FlashcardAIGeneration> FlashcardAIGenerations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Flashcard).Assembly);
    }
}
