﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VkInfrastructure.Data;

namespace VkWeb.Migrations
{
    [DbContext(typeof(VkDbContext))]
    partial class VkDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("VkCore.Models.Invite.OrgInvite", b =>
                {
                    b.Property<string>("OrgId")
                        .HasColumnType("text");

                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<DateTime>("InviteDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("InviteType")
                        .HasColumnType("integer");

                    b.Property<bool>("IsBounced")
                        .HasColumnType("boolean");

                    b.HasKey("OrgId", "Id");

                    b.ToTable("Invites");
                });

            modelBuilder.Entity("VkCore.Models.Organization.Org", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("StudentLimit")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("VkCore.Models.Organization.OrgAdmin", b =>
                {
                    b.Property<string>("OrgId")
                        .HasColumnType("text");

                    b.Property<string>("VkUserId")
                        .HasColumnType("text");

                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.HasKey("OrgId", "VkUserId");

                    b.HasIndex("VkUserId");

                    b.ToTable("OrgAdmins");
                });

            modelBuilder.Entity("VkCore.Models.Organization.OrgReading", b =>
                {
                    b.Property<string>("OrgId")
                        .HasColumnType("text");

                    b.Property<string>("ReadingId")
                        .HasColumnType("text");

                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("VkUserId")
                        .HasColumnType("text");

                    b.HasKey("OrgId", "ReadingId");

                    b.HasIndex("ReadingId");

                    b.HasIndex("VkUserId");

                    b.ToTable("OrgReadings");
                });

            modelBuilder.Entity("VkCore.Models.Organization.OrgStudent", b =>
                {
                    b.Property<string>("OrgId")
                        .HasColumnType("text");

                    b.Property<string>("VkUserId")
                        .HasColumnType("text");

                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.HasKey("OrgId", "VkUserId");

                    b.HasIndex("VkUserId");

                    b.ToTable("OrgStudents");
                });

            modelBuilder.Entity("VkCore.Models.Organization.OrgTeacher", b =>
                {
                    b.Property<string>("OrgId")
                        .HasColumnType("text");

                    b.Property<string>("VkUserId")
                        .HasColumnType("text");

                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("boolean");

                    b.HasKey("OrgId", "VkUserId");

                    b.HasIndex("VkUserId");

                    b.ToTable("OrgTeachers");
                });

            modelBuilder.Entity("VkCore.Models.ReadingModel.ContentItem", b =>
                {
                    b.Property<string>("ReadingId")
                        .HasColumnType("text");

                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AnnotationContextId")
                        .HasColumnType("text");

                    b.Property<string>("AnnotationId")
                        .HasColumnType("text");

                    b.Property<int>("BodyIndex")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.Property<string>("WordId")
                        .HasColumnType("text");

                    b.HasKey("ReadingId", "Id");

                    b.HasIndex("AnnotationId");

                    b.HasIndex("WordId");

                    b.ToTable("ContentItems");
                });

            modelBuilder.Entity("VkCore.Models.ReadingModel.Reading", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("OrgId")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Readings");
                });

            modelBuilder.Entity("VkCore.Models.TagModel.ReadingTag", b =>
                {
                    b.Property<string>("ReadingId")
                        .HasColumnType("text");

                    b.Property<string>("TagId")
                        .HasColumnType("text");

                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.HasKey("ReadingId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("ReadingTag");
                });

            modelBuilder.Entity("VkCore.Models.TagModel.Tag", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("boolean");

                    b.Property<string>("OrgId")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OrgId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("VkCore.Models.TagModel.UserTag", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("TagId")
                        .HasColumnType("text");

                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.HasKey("UserId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("UserTags");
                });

            modelBuilder.Entity("VkCore.Models.VkUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<long?>("FacebookId")
                        .HasColumnType("bigint");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("PictureUrl")
                        .HasColumnType("text");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("VkCore.Models.Word.Annotation", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("PartOfSpeech")
                        .HasColumnType("text");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.Property<string>("WordEntryId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("WordEntryId");

                    b.ToTable("Annotations");
                });

            modelBuilder.Entity("VkCore.Models.Word.AnnotationContext", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AnnotationId")
                        .HasColumnType("text");

                    b.Property<string>("ContentItemId")
                        .HasColumnType("text");

                    b.Property<string>("ReadingId")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AnnotationId");

                    b.ToTable("AnnotationContexts");
                });

            modelBuilder.Entity("VkCore.Models.Word.StudentWord", b =>
                {
                    b.Property<string>("VkUserId")
                        .HasColumnType("text");

                    b.Property<string>("WordEntryId")
                        .HasColumnType("text");

                    b.Property<string>("AnnotationId")
                        .HasColumnType("text");

                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("IsKnown")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("IsKnownDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("VkUserId", "WordEntryId", "AnnotationId");

                    b.HasIndex("AnnotationId");

                    b.HasIndex("WordEntryId");

                    b.ToTable("StudentWords");
                });

            modelBuilder.Entity("VkCore.Models.Word.WordAttempt", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("AttemptDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("AttemptType")
                        .HasColumnType("text");

                    b.Property<string>("StudentWordId")
                        .HasColumnType("text");

                    b.Property<bool>("WasSuccessful")
                        .HasColumnType("boolean");

                    b.Property<string>("WordAnnotationId")
                        .HasColumnType("text");

                    b.Property<string>("WordEntryId")
                        .HasColumnType("text");

                    b.Property<string>("WordVkUserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("WordVkUserId", "WordEntryId", "WordAnnotationId");

                    b.ToTable("WordAttempt");
                });

            modelBuilder.Entity("VkCore.Models.Word.WordEntry", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Word")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Words");
                });

            modelBuilder.Entity("VkCore.Models.Invite.OrgInvite", b =>
                {
                    b.HasOne("VkCore.Models.Organization.Org", null)
                        .WithMany("Invites")
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VkCore.Models.Organization.OrgAdmin", b =>
                {
                    b.HasOne("VkCore.Models.Organization.Org", "Org")
                        .WithMany("Admins")
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VkCore.Models.VkUser", "AdminUser")
                        .WithMany("AdminOrgs")
                        .HasForeignKey("VkUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VkCore.Models.Organization.OrgReading", b =>
                {
                    b.HasOne("VkCore.Models.Organization.Org", "Org")
                        .WithMany("Readings")
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VkCore.Models.ReadingModel.Reading", "Reading")
                        .WithMany("OrgReadings")
                        .HasForeignKey("ReadingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VkCore.Models.VkUser", null)
                        .WithMany("Readings")
                        .HasForeignKey("VkUserId");
                });

            modelBuilder.Entity("VkCore.Models.Organization.OrgStudent", b =>
                {
                    b.HasOne("VkCore.Models.Organization.Org", "Org")
                        .WithMany("Students")
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VkCore.Models.VkUser", "StudentUser")
                        .WithMany("StudentOrgs")
                        .HasForeignKey("VkUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VkCore.Models.Organization.OrgTeacher", b =>
                {
                    b.HasOne("VkCore.Models.Organization.Org", "Org")
                        .WithMany("Teachers")
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VkCore.Models.VkUser", "TeacherUser")
                        .WithMany("TeacherOrgs")
                        .HasForeignKey("VkUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VkCore.Models.ReadingModel.ContentItem", b =>
                {
                    b.HasOne("VkCore.Models.Word.Annotation", "Annotation")
                        .WithMany()
                        .HasForeignKey("AnnotationId");

                    b.HasOne("VkCore.Models.ReadingModel.Reading", null)
                        .WithMany("ContentItems")
                        .HasForeignKey("ReadingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VkCore.Models.Word.WordEntry", "Word")
                        .WithMany()
                        .HasForeignKey("WordId");
                });

            modelBuilder.Entity("VkCore.Models.TagModel.ReadingTag", b =>
                {
                    b.HasOne("VkCore.Models.ReadingModel.Reading", "Reading")
                        .WithMany("Tags")
                        .HasForeignKey("ReadingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VkCore.Models.TagModel.Tag", "Tag")
                        .WithMany("Readings")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VkCore.Models.TagModel.Tag", b =>
                {
                    b.HasOne("VkCore.Models.Organization.Org", null)
                        .WithMany("Tags")
                        .HasForeignKey("OrgId");
                });

            modelBuilder.Entity("VkCore.Models.TagModel.UserTag", b =>
                {
                    b.HasOne("VkCore.Models.TagModel.Tag", "Tag")
                        .WithMany("Users")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VkCore.Models.VkUser", "User")
                        .WithMany("UserTags")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VkCore.Models.Word.Annotation", b =>
                {
                    b.HasOne("VkCore.Models.Word.WordEntry", null)
                        .WithMany("Annotations")
                        .HasForeignKey("WordEntryId");
                });

            modelBuilder.Entity("VkCore.Models.Word.AnnotationContext", b =>
                {
                    b.HasOne("VkCore.Models.Word.Annotation", null)
                        .WithMany("AnnotationContexts")
                        .HasForeignKey("AnnotationId");
                });

            modelBuilder.Entity("VkCore.Models.Word.StudentWord", b =>
                {
                    b.HasOne("VkCore.Models.Word.Annotation", "Annotation")
                        .WithMany("Students")
                        .HasForeignKey("AnnotationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VkCore.Models.VkUser", "User")
                        .WithMany("UserWords")
                        .HasForeignKey("VkUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VkCore.Models.Word.WordEntry", "WordEntry")
                        .WithMany("Students")
                        .HasForeignKey("WordEntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VkCore.Models.Word.WordAttempt", b =>
                {
                    b.HasOne("VkCore.Models.Word.StudentWord", "Word")
                        .WithMany("Attempts")
                        .HasForeignKey("WordVkUserId", "WordEntryId", "WordAnnotationId");
                });
#pragma warning restore 612, 618
        }
    }
}
