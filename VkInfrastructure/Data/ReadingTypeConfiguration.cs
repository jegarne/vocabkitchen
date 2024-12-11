using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VkCore.Models.ReadingModel;
using VkCore.Models.TagModel;

namespace VkInfrastructure.Data
{
    public class ReadingTypeConfiguration : IEntityTypeConfiguration<Reading>
    {
        public void Configure(EntityTypeBuilder<Reading> entity)
        {
            entity.HasMany(p => p.ContentItems)
                .WithOne()
                .HasForeignKey(p => p.ReadingId);

            entity.Metadata
                .FindNavigation(nameof(Reading.ContentItems))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.Metadata
                .FindNavigation(nameof(Reading.Tags))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }

    public class ReadingTagTypeConfiguration : IEntityTypeConfiguration<ReadingTag>
    {
        public void Configure(EntityTypeBuilder<ReadingTag> builder)
        {
            builder.HasKey(t => new { t.ReadingId, t.TagId });
            builder.HasOne(x => x.Reading)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.ReadingId);
            builder.HasOne(x => x.Tag)
                .WithMany(x => x.Readings)
                .HasForeignKey(x => x.TagId);
        }
    }

    public class UserTagTypeConfiguration : IEntityTypeConfiguration<UserTag>
    {
        public void Configure(EntityTypeBuilder<UserTag> builder)
        {
            builder.HasKey(t => new { t.UserId, t.TagId });
            builder.HasOne(x => x.User)
                .WithMany(x => x.UserTags)
                .HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Tag)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.TagId);
        }
    }

    public class TagTypeConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Metadata
                .FindNavigation(nameof(Tag.Readings))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata
                .FindNavigation(nameof(Tag.Users))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
