using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace moongroup.Data.Migrations
{
    public partial class projectsectionstatustable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectFormSection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ProjectSettingId = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFormSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectFormSection_FormSections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "FormSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectFormSection_ProjectsSettings_ProjectSettingId",
                        column: x => x.ProjectSettingId,
                        principalTable: "ProjectsSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFormSection_ProjectSettingId",
                table: "ProjectFormSection",
                column: "ProjectSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFormSection_SectionId",
                table: "ProjectFormSection",
                column: "SectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectFormSection");
        }
    }
}
