using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.Migrations
{
    /// <inheritdoc />
    public partial class countrylanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CountryLanguages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MultiSelectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryLanguages_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryLanguages_MultiSelects_MultiSelectId",
                        column: x => x.MultiSelectId,
                        principalTable: "MultiSelects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountryLanguages_CountryId_MultiSelectId",
                table: "CountryLanguages",
                columns: new[] { "CountryId", "MultiSelectId" });

            migrationBuilder.CreateIndex(
                name: "IX_CountryLanguages_MultiSelectId",
                table: "CountryLanguages",
                column: "MultiSelectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountryLanguages");
        }
    }
}
