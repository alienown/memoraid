using Memoraid.WebApi.Persistence.Entities;
using Memoraid.WebApi.Persistence.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Memoraid.WebApi.Persistence.EntityConfigurations;

public class FlashcardConfiguration : EntityBaseConfiguration<Flashcard>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Flashcard> builder)
    {
        builder.ToTable("flashcards");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.FlashcardAIGenerationId)
            .HasColumnName("flashcard_ai_generation_id");

        builder.HasOne<FlashcardAIGeneration>()
            .WithMany()
            .HasForeignKey(x => x.FlashcardAIGenerationId)
            .IsRequired(false);

        builder.Property(x => x.Source)
            .HasConversion(new EnumToStringConverter<FlashcardSource>());
    }
}