using Microsoft.EntityFrameworkCore.Migrations;

namespace moongroup.Data.Migrations
{
    public partial class add_form_field_id_form_field_change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormFields_FormFields_FormField",
                table: "FormFields");

            migrationBuilder.RenameColumn(
                name: "FormField",
                table: "FormFields",
                newName: "FormFieldId");

            migrationBuilder.RenameIndex(
                name: "IX_FormFields_FormField",
                table: "FormFields",
                newName: "IX_FormFields_FormFieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormFields_FormFields_FormFieldId",
                table: "FormFields",
                column: "FormFieldId",
                principalTable: "FormFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormFields_FormFields_FormFieldId",
                table: "FormFields");

            migrationBuilder.RenameColumn(
                name: "FormFieldId",
                table: "FormFields",
                newName: "FormField");

            migrationBuilder.RenameIndex(
                name: "IX_FormFields_FormFieldId",
                table: "FormFields",
                newName: "IX_FormFields_FormField");

            migrationBuilder.AddForeignKey(
                name: "FK_FormFields_FormFields_FormField",
                table: "FormFields",
                column: "FormField",
                principalTable: "FormFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
