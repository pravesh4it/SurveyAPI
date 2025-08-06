using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.Migrations
{
    /// <inheritdoc />
    public partial class Designation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DesignationId",
                table: "UserInfoes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Designation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInfoes_DesignationId",
                table: "UserInfoes",
                column: "DesignationId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInfoes_Designation_DesignationId",
                table: "UserInfoes",
                column: "DesignationId",
                principalTable: "Designation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInfoes_Designation_DesignationId",
                table: "UserInfoes");

            migrationBuilder.DropTable(
                name: "Designation");

            migrationBuilder.DropIndex(
                name: "IX_UserInfoes_DesignationId",
                table: "UserInfoes");

            migrationBuilder.DropColumn(
                name: "DesignationId",
                table: "UserInfoes");
        }
    }
}
