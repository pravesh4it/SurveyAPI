using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.Migrations
{
    /// <inheritdoc />
    public partial class addcolumnsurresp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AddedBy",
                table: "surveyResponses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "DeviceType",
                table: "surveyResponses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternalStatus",
                table: "surveyResponses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusNotes",
                table: "surveyResponses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "surveyResponses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PreScreener",
                table: "partnerSurveys",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "surveyResponses");

            migrationBuilder.DropColumn(
                name: "InternalStatus",
                table: "surveyResponses");

            migrationBuilder.DropColumn(
                name: "StatusNotes",
                table: "surveyResponses");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "surveyResponses");

            migrationBuilder.DropColumn(
                name: "PreScreener",
                table: "partnerSurveys");

            migrationBuilder.AlterColumn<string>(
                name: "AddedBy",
                table: "surveyResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
