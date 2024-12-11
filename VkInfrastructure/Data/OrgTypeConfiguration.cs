using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VkCore.Models.Organization;

namespace VkInfrastructure.Data
{
    public class OrgTypeConfiguration : IEntityTypeConfiguration<Org>
    {
        public void Configure(EntityTypeBuilder<Org> builder)
        {
            builder.Metadata
                .FindNavigation(nameof(Org.Admins))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata
                .FindNavigation(nameof(Org.Teachers))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata
                .FindNavigation(nameof(Org.Students))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata
                .FindNavigation(nameof(Org.Invites))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata
                .FindNavigation(nameof(Org.Readings))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata
                .FindNavigation(nameof(Org.Tags))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }

    public class OrgAdminTypeConfiguration : IEntityTypeConfiguration<OrgAdmin>
    {
        public void Configure(EntityTypeBuilder<OrgAdmin> builder)
        {
            builder.HasKey(t => new {t.OrgId, t.VkUserId});
            builder.HasOne(x => x.AdminUser)
                .WithMany(x => x.AdminOrgs)
                .HasForeignKey(x => x.VkUserId);
            builder.HasOne(x => x.Org)
                .WithMany(x => x.Admins)
                .HasForeignKey(x => x.OrgId);
        }
    }

    public class OrgTeacherTypeConfiguration : IEntityTypeConfiguration<OrgTeacher>
    {
        public void Configure(EntityTypeBuilder<OrgTeacher> builder)
        {
            builder.HasKey(t => new {t.OrgId, t.VkUserId});
            builder.HasOne(x => x.TeacherUser)
                .WithMany(x => x.TeacherOrgs)
                .HasForeignKey(x => x.VkUserId);
            builder.HasOne(x => x.Org)
                .WithMany(x => x.Teachers)
                .HasForeignKey(x => x.OrgId);
        }
    }

    public class OrgStudentTypeConfiguration : IEntityTypeConfiguration<OrgStudent>
    {
        public void Configure(EntityTypeBuilder<OrgStudent> builder)
        {
            builder.HasKey(t => new {t.OrgId, t.VkUserId});
            builder.HasOne(x => x.StudentUser)
                .WithMany(x => x.StudentOrgs)
                .HasForeignKey(x => x.VkUserId);
            builder.HasOne(x => x.Org)
                .WithMany(x => x.Students)
                .HasForeignKey(x => x.OrgId);
        }
    }

    public class OrgReadingTypeConfiguration : IEntityTypeConfiguration<OrgReading>
    {
        public void Configure(EntityTypeBuilder<OrgReading> builder)
        {
            builder.HasKey(t => new { t.OrgId, t.ReadingId });
            builder.HasOne(x => x.Reading)
                .WithMany(x => x.OrgReadings)
                .HasForeignKey(x => x.ReadingId);
            builder.HasOne(x => x.Org)
                .WithMany(x => x.Readings)
                .HasForeignKey(x => x.OrgId);
        }
    }
}
