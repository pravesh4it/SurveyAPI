using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.Migrations
{
    /// <inheritdoc />
    public partial class adduniquelinks2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyFile_Surveys_SurveyId",
                table: "SurveyFile");

            migrationBuilder.DropForeignKey(
                name: "FK_surveyResponses_SurveyFile_SurveyFileId",
                table: "surveyResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyFile",
                table: "SurveyFile");

            migrationBuilder.RenameTable(
                name: "SurveyFile",
                newName: "surveyFile");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyFile_SurveyId",
                table: "surveyFile",
                newName: "IX_surveyFile_SurveyId");

            migrationBuilder.AddColumn<string>(
                name: "FileName_show",
                table: "surveyFile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_surveyFile",
                table: "surveyFile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_surveyFile_Surveys_SurveyId",
                table: "surveyFile",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_surveyResponses_surveyFile_SurveyFileId",
                table: "surveyResponses",
                column: "SurveyFileId",
                principalTable: "surveyFile",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_surveyFile_Surveys_SurveyId",
                table: "surveyFile");

            migrationBuilder.DropForeignKey(
                name: "FK_surveyResponses_surveyFile_SurveyFileId",
                table: "surveyResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_surveyFile",
                table: "surveyFile");

            migrationBuilder.DropColumn(
                name: "FileName_show",
                table: "surveyFile");

            migrationBuilder.RenameTable(
                name: "surveyFile",
                newName: "SurveyFile");

            migrationBuilder.RenameIndex(
                name: "IX_surveyFile_SurveyId",
                table: "SurveyFile",
                newName: "IX_SurveyFile_SurveyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyFile",
                table: "SurveyFile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyFile_Surveys_SurveyId",
                table: "SurveyFile",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_surveyResponses_SurveyFile_SurveyFileId",
                table: "surveyResponses",
                column: "SurveyFileId",
                principalTable: "SurveyFile",
                principalColumn: "Id");
        }
    }
}
