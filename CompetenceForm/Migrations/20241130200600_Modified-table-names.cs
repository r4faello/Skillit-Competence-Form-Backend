using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompetenceForm.Migrations
{
    /// <inheritdoc />
    public partial class Modifiedtablenames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Draft_CompetenceSets_CompetenceSetId",
                table: "Draft");

            migrationBuilder.DropForeignKey(
                name: "FK_Draft_Users_AuthorId",
                table: "Draft");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswer_Answers_AnswerId",
                table: "QuestionAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswer_Draft_DraftId",
                table: "QuestionAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswer_Questions_QuestionId",
                table: "QuestionAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionAnswer",
                table: "QuestionAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Draft",
                table: "Draft");

            migrationBuilder.RenameTable(
                name: "QuestionAnswer",
                newName: "QuestionAnswerPairs");

            migrationBuilder.RenameTable(
                name: "Draft",
                newName: "Drafts");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionAnswer_QuestionId",
                table: "QuestionAnswerPairs",
                newName: "IX_QuestionAnswerPairs_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionAnswer_DraftId",
                table: "QuestionAnswerPairs",
                newName: "IX_QuestionAnswerPairs_DraftId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionAnswer_AnswerId",
                table: "QuestionAnswerPairs",
                newName: "IX_QuestionAnswerPairs_AnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_Draft_CompetenceSetId",
                table: "Drafts",
                newName: "IX_Drafts_CompetenceSetId");

            migrationBuilder.RenameIndex(
                name: "IX_Draft_AuthorId",
                table: "Drafts",
                newName: "IX_Drafts_AuthorId");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionId",
                table: "QuestionAnswerPairs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Drafts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionAnswerPairs",
                table: "QuestionAnswerPairs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Drafts",
                table: "Drafts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Drafts_CompetenceSets_CompetenceSetId",
                table: "Drafts",
                column: "CompetenceSetId",
                principalTable: "CompetenceSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Drafts_Users_AuthorId",
                table: "Drafts",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswerPairs_Answers_AnswerId",
                table: "QuestionAnswerPairs",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswerPairs_Drafts_DraftId",
                table: "QuestionAnswerPairs",
                column: "DraftId",
                principalTable: "Drafts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswerPairs_Questions_QuestionId",
                table: "QuestionAnswerPairs",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drafts_CompetenceSets_CompetenceSetId",
                table: "Drafts");

            migrationBuilder.DropForeignKey(
                name: "FK_Drafts_Users_AuthorId",
                table: "Drafts");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswerPairs_Answers_AnswerId",
                table: "QuestionAnswerPairs");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswerPairs_Drafts_DraftId",
                table: "QuestionAnswerPairs");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswerPairs_Questions_QuestionId",
                table: "QuestionAnswerPairs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionAnswerPairs",
                table: "QuestionAnswerPairs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Drafts",
                table: "Drafts");

            migrationBuilder.RenameTable(
                name: "QuestionAnswerPairs",
                newName: "QuestionAnswer");

            migrationBuilder.RenameTable(
                name: "Drafts",
                newName: "Draft");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionAnswerPairs_QuestionId",
                table: "QuestionAnswer",
                newName: "IX_QuestionAnswer_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionAnswerPairs_DraftId",
                table: "QuestionAnswer",
                newName: "IX_QuestionAnswer_DraftId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionAnswerPairs_AnswerId",
                table: "QuestionAnswer",
                newName: "IX_QuestionAnswer_AnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_Drafts_CompetenceSetId",
                table: "Draft",
                newName: "IX_Draft_CompetenceSetId");

            migrationBuilder.RenameIndex(
                name: "IX_Drafts_AuthorId",
                table: "Draft",
                newName: "IX_Draft_AuthorId");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionId",
                table: "QuestionAnswer",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Draft",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionAnswer",
                table: "QuestionAnswer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Draft",
                table: "Draft",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Draft_CompetenceSets_CompetenceSetId",
                table: "Draft",
                column: "CompetenceSetId",
                principalTable: "CompetenceSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Draft_Users_AuthorId",
                table: "Draft",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswer_Answers_AnswerId",
                table: "QuestionAnswer",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswer_Draft_DraftId",
                table: "QuestionAnswer",
                column: "DraftId",
                principalTable: "Draft",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswer_Questions_QuestionId",
                table: "QuestionAnswer",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
