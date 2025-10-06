using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.Migrations
{
    /// <inheritdoc />
    public partial class adduniquelink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UniqueLink",
                table: "Surveys",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueLink",
                table: "Surveys");
        }
    }
}
