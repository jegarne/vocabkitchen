using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VkCore.Models.Word;

namespace VkInfrastructure.Data
{
    public class WordEntryTypeConfiguration : IEntityTypeConfiguration<WordEntry>
    {
        public void Configure(EntityTypeBuilder<WordEntry> entity)
        {
            entity.Metadata
                .FindNavigation(nameof(WordEntry.Annotations))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }

    public class AnnotationTypeConfiguration : IEntityTypeConfiguration<Annotation>
    {
        public void Configure(EntityTypeBuilder<Annotation> entity)
        {
            entity.Metadata
                .FindNavigation(nameof(Annotation.AnnotationContexts))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }

    public class StudentWordTypeConfiguration : IEntityTypeConfiguration<StudentWord>
    {
        public void Configure(EntityTypeBuilder<StudentWord> builder)
        {
            builder.Ignore(w => w.ClozeAttempts);
            builder.Ignore(w => w.MeaningAttempts);
            builder.Ignore(w => w.SpellingAttempts);
            
            builder.Metadata
                .FindNavigation(nameof(StudentWord.Attempts))
                .SetPropertyAccessMode(PropertyAccessMode.Field);



            builder.HasKey(t => new { t.VkUserId, t.WordEntryId, t.AnnotationId });
            builder.HasOne(x => x.User)
                .WithMany(x => x.UserWords)
                .HasForeignKey(x => x.VkUserId);
            builder.HasOne(x => x.WordEntry)
                .WithMany(x => x.Students)
                .HasForeignKey(x => x.WordEntryId);
            builder.HasOne(x => x.Annotation)
                .WithMany(x => x.Students)
                .HasForeignKey(x => x.AnnotationId);
        }
    }
}
