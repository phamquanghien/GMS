using Microsoft.EntityFrameworkCore.Migrations;

namespace GSM.Migrations
{
    public partial class Add_InvoiceDetail_CategoryID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "InvoiceDetails",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "InvoiceDetails");
        }
    }
}
