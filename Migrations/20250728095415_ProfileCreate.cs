using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.Migrations
{
    /// <inheritdoc />
    public partial class ProfileCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Income = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Children = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HouseholdSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EducationLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmploymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Devices = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShoppingFrequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DomesticTravel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternationalTravel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediaPlatforms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HealthStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Chronic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmokingAlcohol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Exercise = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Diet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurveyLength = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurveyFormats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurveyFrequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RewardType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RewardFrequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferralSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcceptTerms = table.Column<bool>(type: "bit", nullable: false),
                    AcceptPrivacy = table.Column<bool>(type: "bit", nullable: false),
                    AcceptDataProcessing = table.Column<bool>(type: "bit", nullable: false),
                    SubscribeNewsletter = table.Column<bool>(type: "bit", nullable: false),
                    ProfileURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfiles");
        }
    }
}
