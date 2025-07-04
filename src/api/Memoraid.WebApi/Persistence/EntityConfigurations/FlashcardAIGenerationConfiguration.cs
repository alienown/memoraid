using Memoraid.WebApi.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoraid.WebApi.Persistence.EntityConfigurations;

public class FlashcardAIGenerationConfiguration : EntityBaseConfiguration<FlashcardAIGeneration>
{
    protected override void ConfigureEntity(EntityTypeBuilder<FlashcardAIGeneration> builder)
    {
        builder.ToTable("flashcard_ai_generations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.AIModel)
            .HasColumnName("ai_model");
    }
}