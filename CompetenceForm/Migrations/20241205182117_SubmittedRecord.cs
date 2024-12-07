using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompetenceForm.Migrations
{
    /// <inheritdoc />
    public partial class SubmittedRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubmittedRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompetenceSetId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmittedRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubmittedRecords_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompetenceValues",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompetenceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Value = table.Column<int>(type: "int", nullable: true),
                    SubmittedRecordId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetenceValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetenceValues_Competences_CompetenceId",
                        column: x => x.CompetenceId,
                        principalTable: "Competences",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompetenceValues_SubmittedRecords_SubmittedRecordId",
                        column: x => x.SubmittedRecordId,
                        principalTable: "SubmittedRecords",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetenceValues_CompetenceId",
                table: "CompetenceValues",
                column: "CompetenceId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetenceValues_SubmittedRecordId",
                table: "CompetenceValues",
                column: "SubmittedRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmittedRecords_AuthorId",
                table: "SubmittedRecords",
                column: "AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetenceValues");

            migrationBuilder.DropTable(
                name: "SubmittedRecords");
        }
    }
}
