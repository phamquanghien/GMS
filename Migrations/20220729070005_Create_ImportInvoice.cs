using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GSM.Migrations
{
    public partial class Create_ImportInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportInvoice",
                columns: table => new
                {
                    ImportInvoiceID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalMoney = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportInvoice", x => x.ImportInvoiceID);
                });

            migrationBuilder.CreateTable(
                name: "ImportInvoiceDetails",
                columns: table => new
                {
                    InvoiceDetailID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImportInvoiceID = table.Column<int>(type: "INTEGER", nullable: true),
                    CategoryID = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: true),
                    SumWeight = table.Column<double>(type: "REAL", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true),
                    GoldUnitPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    IntoMoney = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportInvoiceDetails", x => x.InvoiceDetailID);
                    table.ForeignKey(
                        name: "FK_ImportInvoiceDetails_ImportInvoice_ImportInvoiceID",
                        column: x => x.ImportInvoiceID,
                        principalTable: "ImportInvoice",
                        principalColumn: "ImportInvoiceID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImportInvoiceDetails_ImportInvoiceID",
                table: "ImportInvoiceDetails",
                column: "ImportInvoiceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportInvoiceDetails");

            migrationBuilder.DropTable(
                name: "ImportInvoice");
        }
    }
}
