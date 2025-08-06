using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.Migrations
{
    /// <inheritdoc />
    public partial class AddSurveyParentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Surveys",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_ParentId",
                table: "Surveys",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Surveys_Surveys_ParentId",
                table: "Surveys",
                column: "ParentId",
                principalTable: "Surveys",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Surveys_Surveys_ParentId",
                table: "Surveys");

            migrationBuilder.DropIndex(
                name: "IX_Surveys_ParentId",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Surveys");
        }
    }
}
