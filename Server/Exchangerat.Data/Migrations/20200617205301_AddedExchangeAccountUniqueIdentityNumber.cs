using Microsoft.EntityFrameworkCore.Migrations;

namespace Exchangerat.Data.Migrations
{
    public partial class AddedExchangeAccountUniqueIdentityNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ExchangeAccounts_IdentityNumber",
                table: "ExchangeAccounts",
                column: "IdentityNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExchangeAccounts_IdentityNumber",
                table: "ExchangeAccounts");
        }
    }
}
