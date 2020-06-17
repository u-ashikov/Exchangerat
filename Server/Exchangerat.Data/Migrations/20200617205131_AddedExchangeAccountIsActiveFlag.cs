using Microsoft.EntityFrameworkCore.Migrations;

namespace Exchangerat.Data.Migrations
{
    public partial class AddedExchangeAccountIsActiveFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExchangeAccounts",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExchangeAccounts");
        }
    }
}
