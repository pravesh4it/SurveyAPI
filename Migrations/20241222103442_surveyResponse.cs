using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.Migrations
{
    /// <inheritdoc />
    public partial class surveyResponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisqualificationLink",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PausedLink",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuotaFullLink",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecurityFailLink",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SuccessLink",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "partnerSurveys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvailableVariable = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartnerSuccessLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartnerDisqualificationLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartnerQuotaLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PausedLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecurityFailLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurveyUuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partnerSurveys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "surveyResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RespondentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResponseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompletionTimeInMinutes = table.Column<int>(type: "int", nullable: true),
                    Answers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RespondentIP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartnerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_surveyResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_surveyResponses_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_surveyResponses_SurveyId",
                table: "surveyResponses",
                column: "SurveyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "partnerSurveys");

            migrationBuilder.DropTable(
                name: "surveyResponses");

            migrationBuilder.DropColumn(
                name: "DisqualificationLink",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "PausedLink",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "QuotaFullLink",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "SecurityFailLink",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "SuccessLink",
                table: "Clients");
        }
    }
}
