using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.Migrations
{
    /// <inheritdoc />
    public partial class invoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvoiceMaster",
                columns: table => new
                {
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    SurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClientSurveyName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PONumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccountEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AddrLine1 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    AddrLine2 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    AddrLine3 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentDone = table.Column<bool>(type: "bit", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentTermsDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GrandTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TaxTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    DiscountTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    AdditionalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceMaster", x => x.InvoiceId);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceTransaction",
                columns: table => new
                {
                    InvoiceTxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineNo = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false, defaultValue: 1m),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitCost = table.Column<decimal>(type: "decimal(18,4)", nullable: false, defaultValue: 0m),
                    LineTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceTransaction", x => x.InvoiceTxId);
                    table.ForeignKey(
                        name: "FK_InvoiceTransaction_InvoiceMaster_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "InvoiceMaster",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceMaster_InvoiceNumber",
                table: "InvoiceMaster",
                column: "InvoiceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceMaster_SurveyId",
                table: "InvoiceMaster",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTransaction_InvoiceId_LineNo",
                table: "InvoiceTransaction",
                columns: new[] { "InvoiceId", "LineNo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceTransaction");

            migrationBuilder.DropTable(
                name: "InvoiceMaster");
        }
    }
}
