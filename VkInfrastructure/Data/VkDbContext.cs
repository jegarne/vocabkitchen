using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Interfaces;
using VkCore.Models;
using VkCore.Models.Invite;
using VkCore.Models.Organization;
using VkCore.Models.ReadingModel;
using VkCore.Models.TagModel;
using VkCore.Models.Word;
using VkCore.SharedKernel;

namespace VkInfrastructure.Data
{
    public class VkDbContext : DbContext
    {
        private readonly IMediator _mediator;

        public VkDbContext(DbContextOptions<VkDbContext> options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<Org> Organizations { get; set; }
        public DbSet<VkUser> Users { get; set; }
        public DbSet<OrgTeacher> OrgTeachers { get; set; }
        public DbSet<OrgInvite> Invites { get; set; }
        public DbSet<Reading> Readings { get; set; }
        public DbSet<WordEntry> Words { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<UserTag> UserTags { get; set; }
        public DbSet<StudentWord> StudentWords { get; set; }
        public DbSet<ContentItem> ContentItems { get; set; }
        public DbSet<Annotation> Annotations { get; set; }
        public DbSet<AnnotationContext> AnnotationContexts { get; set; }
        public DbSet<OrgReading> OrgReadings { get; set; }
        public DbSet<OrgAdmin> OrgAdmins { get; set; }
        public DbSet<OrgStudent> OrgStudents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // org relationships
            modelBuilder.ApplyConfiguration(new OrgTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrgAdminTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrgTeacherTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrgStudentTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrgReadingTypeConfiguration());

            // reading relationships
            modelBuilder.ApplyConfiguration(new ReadingTypeConfiguration());

            // word relationships
            modelBuilder.ApplyConfiguration(new WordEntryTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AnnotationTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StudentWordTypeConfiguration());

            // tag relationships
            modelBuilder.ApplyConfiguration(new TagTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReadingTagTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserTagTypeConfiguration());

            // one to many foreign keys
            modelBuilder.Entity<OrgInvite>().HasKey(t => new { t.OrgId, t.Id });
            modelBuilder.Entity<ContentItem>().HasKey(t => new { t.ReadingId, t.Id });

            // filter out soft deletes globally
            modelBuilder.Entity<Reading>().HasQueryFilter(p => !p.IsDeleted);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            var markedAsDeleted = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);

            foreach (var item in markedAsDeleted)
            {
                if (item.Entity is IIsSoftDeleted entity)
                {
                    // Set the entity to unchanged (if we mark the whole entity as Modified, every field gets sent to Db as an update)
                    item.State = EntityState.Unchanged;
                    // Only update the IsDeleted flag - only this will get sent to the Db
                    entity.IsDeleted = true;
                }
            }

            int result = base.SaveChanges();

            // dispatch events only if save was successful
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.Events.ToArray();
                entity.ClearEvents();
                foreach (var domainEvent in events)
                {
                    _mediator.Publish(domainEvent);
                }
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.DetectChanges();

            var markedAsDeleted = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);

            foreach (var item in markedAsDeleted)
            {
                if (item.Entity is IIsSoftDeleted entity)
                {
                    // Set the entity to unchanged (if we mark the whole entity as Modified, every field gets sent to Db as an update)
                    item.State = EntityState.Unchanged;
                    // Only update the IsDeleted flag - only this will get sent to the Db
                    entity.IsDeleted = true;
                }
            }

            int result = await base.SaveChangesAsync(cancellationToken);

            // dispatch events only if save was successful
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.Events.ToArray();
                entity.ClearEvents();
                foreach (var domainEvent in events)
                {
                    await _mediator.Publish(domainEvent, cancellationToken);
                }
            }

            return result;
        }

    }
}
