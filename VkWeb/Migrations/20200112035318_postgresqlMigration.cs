using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VkWeb.Migrations
{
    public partial class postgresqlMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Readings",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    OrgId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    FacebookId = table.Column<long>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Word = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Invites",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    OrgId = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    InviteType = table.Column<int>(nullable: false),
                    InviteDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invites", x => new { x.OrgId, x.Id });
                    table.ForeignKey(
                        name: "FK_Invites_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    OrgId = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrgAdmin",
                columns: table => new
                {
                    OrgId = table.Column<string>(nullable: false),
                    VkUserId = table.Column<string>(nullable: false),
                    Id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgAdmin", x => new { x.OrgId, x.VkUserId });
                    table.ForeignKey(
                        name: "FK_OrgAdmin_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgAdmin_Users_VkUserId",
                        column: x => x.VkUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrgReading",
                columns: table => new
                {
                    OrgId = table.Column<string>(nullable: false),
                    ReadingId = table.Column<string>(nullable: false),
                    Id = table.Column<string>(nullable: true),
                    VkUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgReading", x => new { x.OrgId, x.ReadingId });
                    table.ForeignKey(
                        name: "FK_OrgReading_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgReading_Readings_ReadingId",
                        column: x => x.ReadingId,
                        principalTable: "Readings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgReading_Users_VkUserId",
                        column: x => x.VkUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrgStudent",
                columns: table => new
                {
                    OrgId = table.Column<string>(nullable: false),
                    VkUserId = table.Column<string>(nullable: false),
                    Id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgStudent", x => new { x.OrgId, x.VkUserId });
                    table.ForeignKey(
                        name: "FK_OrgStudent_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgStudent_Users_VkUserId",
                        column: x => x.VkUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrgTeachers",
                columns: table => new
                {
                    OrgId = table.Column<string>(nullable: false),
                    VkUserId = table.Column<string>(nullable: false),
                    Id = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgTeachers", x => new { x.OrgId, x.VkUserId });
                    table.ForeignKey(
                        name: "FK_OrgTeachers_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgTeachers_Users_VkUserId",
                        column: x => x.VkUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Annotations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    WordEntryId = table.Column<string>(nullable: true),
                    PartOfSpeech = table.Column<string>(nullable: true),
                    Source = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Annotations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Annotations_Words_WordEntryId",
                        column: x => x.WordEntryId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReadingTag",
                columns: table => new
                {
                    ReadingId = table.Column<string>(nullable: false),
                    TagId = table.Column<string>(nullable: false),
                    Id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingTag", x => new { x.ReadingId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ReadingTag_Readings_ReadingId",
                        column: x => x.ReadingId,
                        principalTable: "Readings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReadingTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTags",
                columns: table => new
                {
                    TagId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTags", x => new { x.UserId, x.TagId });
                    table.ForeignKey(
                        name: "FK_UserTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnnotationContexts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    AnnotationId = table.Column<string>(nullable: true),
                    ReadingId = table.Column<string>(nullable: true),
                    ContentItemId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnotationContexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnotationContexts_Annotations_AnnotationId",
                        column: x => x.AnnotationId,
                        principalTable: "Annotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContentItems",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ReadingId = table.Column<string>(nullable: false),
                    BodyIndex = table.Column<int>(nullable: false),
                    WordId = table.Column<string>(nullable: true),
                    AnnotationId = table.Column<string>(nullable: true),
                    AnnotationContextId = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentItems", x => new { x.ReadingId, x.Id });
                    table.ForeignKey(
                        name: "FK_ContentItems_Annotations_AnnotationId",
                        column: x => x.AnnotationId,
                        principalTable: "Annotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContentItems_Readings_ReadingId",
                        column: x => x.ReadingId,
                        principalTable: "Readings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentItems_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentWords",
                columns: table => new
                {
                    VkUserId = table.Column<string>(nullable: false),
                    WordEntryId = table.Column<string>(nullable: false),
                    AnnotationId = table.Column<string>(nullable: false),
                    Id = table.Column<string>(nullable: true),
                    IsKnown = table.Column<bool>(nullable: false),
                    IsKnownDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentWords", x => new { x.VkUserId, x.WordEntryId, x.AnnotationId });
                    table.ForeignKey(
                        name: "FK_StudentWords_Annotations_AnnotationId",
                        column: x => x.AnnotationId,
                        principalTable: "Annotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentWords_Users_VkUserId",
                        column: x => x.VkUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentWords_Words_WordEntryId",
                        column: x => x.WordEntryId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordAttempt",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AttemptDate = table.Column<DateTime>(nullable: false),
                    AttemptType = table.Column<string>(nullable: true),
                    WasSuccessful = table.Column<bool>(nullable: false),
                    StudentWordId = table.Column<string>(nullable: true),
                    WordVkUserId = table.Column<string>(nullable: true),
                    WordEntryId = table.Column<string>(nullable: true),
                    WordAnnotationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordAttempt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordAttempt_StudentWords_WordVkUserId_WordEntryId_WordAnnot~",
                        columns: x => new { x.WordVkUserId, x.WordEntryId, x.WordAnnotationId },
                        principalTable: "StudentWords",
                        principalColumns: new[] { "VkUserId", "WordEntryId", "AnnotationId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnotationContexts_AnnotationId",
                table: "AnnotationContexts",
                column: "AnnotationId");

            migrationBuilder.CreateIndex(
                name: "IX_Annotations_WordEntryId",
                table: "Annotations",
                column: "WordEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentItems_AnnotationId",
                table: "ContentItems",
                column: "AnnotationId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentItems_WordId",
                table: "ContentItems",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgAdmin_VkUserId",
                table: "OrgAdmin",
                column: "VkUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgReading_ReadingId",
                table: "OrgReading",
                column: "ReadingId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgReading_VkUserId",
                table: "OrgReading",
                column: "VkUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgStudent_VkUserId",
                table: "OrgStudent",
                column: "VkUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgTeachers_VkUserId",
                table: "OrgTeachers",
                column: "VkUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingTag_TagId",
                table: "ReadingTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWords_AnnotationId",
                table: "StudentWords",
                column: "AnnotationId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWords_WordEntryId",
                table: "StudentWords",
                column: "WordEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_OrgId",
                table: "Tags",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTags_TagId",
                table: "UserTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_WordAttempt_WordVkUserId_WordEntryId_WordAnnotationId",
                table: "WordAttempt",
                columns: new[] { "WordVkUserId", "WordEntryId", "WordAnnotationId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnotationContexts");

            migrationBuilder.DropTable(
                name: "ContentItems");

            migrationBuilder.DropTable(
                name: "Invites");

            migrationBuilder.DropTable(
                name: "OrgAdmin");

            migrationBuilder.DropTable(
                name: "OrgReading");

            migrationBuilder.DropTable(
                name: "OrgStudent");

            migrationBuilder.DropTable(
                name: "OrgTeachers");

            migrationBuilder.DropTable(
                name: "ReadingTag");

            migrationBuilder.DropTable(
                name: "UserTags");

            migrationBuilder.DropTable(
                name: "WordAttempt");

            migrationBuilder.DropTable(
                name: "Readings");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "StudentWords");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Annotations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Words");
        }
    }
}
