using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GSM.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    InvoiceID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerName = table.Column<string>(type: "TEXT", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    InvoiceCode = table.Column<string>(type: "TEXT", nullable: true),
                    TotalMoney = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsPaid = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.InvoiceID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDetails",
                columns: table => new
                {
                    InvoiceDetailID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvoiceID = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductName = table.Column<string>(type: "TEXT", nullable: true),
                    SumWeight = table.Column<double>(type: "REAL", nullable: false),
                    PercentGold = table.Column<double>(type: "REAL", nullable: false),
                    GoldWeight = table.Column<double>(type: "REAL", nullable: false),
                    GoldUnitPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    GemstoneWeight = table.Column<double>(type: "REAL", nullable: false),
                    GemstoneUnitPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    CraftingWages = table.Column<decimal>(type: "TEXT", nullable: false),
                    IntoMoney = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDetails", x => x.InvoiceDetailID);
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Invoice_InvoiceID",
                        column: x => x.InvoiceID,
                        principalTable: "Invoice",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_InvoiceID",
                table: "InvoiceDetails",
                column: "InvoiceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceDetails");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Invoice");
        }
    }
}
