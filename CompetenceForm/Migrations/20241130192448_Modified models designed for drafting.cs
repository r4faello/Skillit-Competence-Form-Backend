using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompetenceForm.Migrations
{
    /// <inheritdoc />
    public partial class Modifiedmodelsdesignedfordrafting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Draft",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompetenceSetId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InitiatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Draft", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Draft_CompetenceSets_CompetenceSetId",
                        column: x => x.CompetenceSetId,
                        principalTable: "CompetenceSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Draft_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswer",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuestionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AnswerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DraftId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionAnswer_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionAnswer_Draft_DraftId",
                        column: x => x.DraftId,
                        principalTable: "Draft",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuestionAnswer_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Draft_AuthorId",
                table: "Draft",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Draft_CompetenceSetId",
                table: "Draft",
                column: "CompetenceSetId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswer_AnswerId",
                table: "QuestionAnswer",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswer_DraftId",
                table: "QuestionAnswer",
                column: "DraftId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswer_QuestionId",
                table: "QuestionAnswer",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionAnswer");

            migrationBuilder.DropTable(
                name: "Draft");
        }
    }
}
