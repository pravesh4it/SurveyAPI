using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.Migrations
{
    /// <inheritdoc />
    public partial class adduniquelinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRecontact",
                table: "surveyResponses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SurveyFileId",
                table: "surveyResponses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SurveyFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TotalLinks = table.Column<int>(type: "int", nullable: false),
                    UsedLinks = table.Column<int>(type: "int", nullable: false),
                    RemainingLinks = table.Column<int>(type: "int", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyFile_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_surveyResponses_SurveyFileId",
                table: "surveyResponses",
                column: "SurveyFileId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyFile_SurveyId",
                table: "SurveyFile",
                column: "SurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_surveyResponses_SurveyFile_SurveyFileId",
                table: "surveyResponses",
                column: "SurveyFileId",
                principalTable: "SurveyFile",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_surveyResponses_SurveyFile_SurveyFileId",
                table: "surveyResponses");

            migrationBuilder.DropTable(
                name: "SurveyFile");

            migrationBuilder.DropIndex(
                name: "IX_surveyResponses_SurveyFileId",
                table: "surveyResponses");

            migrationBuilder.DropColumn(
                name: "IsRecontact",
                table: "surveyResponses");

            migrationBuilder.DropColumn(
                name: "SurveyFileId",
                table: "surveyResponses");
        }
    }
}
