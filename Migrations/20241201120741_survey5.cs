using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.Migrations
{
    /// <inheritdoc />
    public partial class survey5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilledTimeInDays = table.Column<int>(type: "int", nullable: false),
                    Completes = table.Column<int>(type: "int", nullable: false),
                    LengthOfSurveyInMinutes = table.Column<int>(type: "int", nullable: false),
                    Incidence = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientOrPartner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Success_Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Disqualification_Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuotaFull_Link = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Surveys_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projectManagers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projectManagers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_projectManagers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_projectManagers_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "salesManagers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salesManagers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_salesManagers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_salesManagers_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_projectManagers_SurveyId",
                table: "projectManagers",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_projectManagers_UserId",
                table: "projectManagers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_salesManagers_SurveyId",
                table: "salesManagers",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_salesManagers_UserId",
                table: "salesManagers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_CreatedById",
                table: "Surveys",
                column: "CreatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "projectManagers");

            migrationBuilder.DropTable(
                name: "salesManagers");

            migrationBuilder.DropTable(
                name: "Surveys");
        }
    }
}
