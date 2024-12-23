using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompetenceForm.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drafts_Users_AuthorId",
                table: "Drafts");

            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedRecords_Users_AuthorId",
                table: "SubmittedRecords");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "SubmittedRecords",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Drafts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Drafts_Users_AuthorId",
                table: "Drafts",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedRecords_Users_AuthorId",
                table: "SubmittedRecords",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drafts_Users_AuthorId",
                table: "Drafts");

            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedRecords_Users_AuthorId",
                table: "SubmittedRecords");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "SubmittedRecords",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Drafts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Drafts_Users_AuthorId",
                table: "Drafts",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedRecords_Users_AuthorId",
                table: "SubmittedRecords",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
