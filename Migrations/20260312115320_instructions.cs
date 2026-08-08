using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.Migrations
{
    /// <inheritdoc />
    public partial class instructions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasInstruction",
                table: "partnerSurveys",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructionText",
                table: "partnerSurveys",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasInstruction",
                table: "partnerSurveys");

            migrationBuilder.DropColumn(
                name: "InstructionText",
                table: "partnerSurveys");
        }
    }
}
