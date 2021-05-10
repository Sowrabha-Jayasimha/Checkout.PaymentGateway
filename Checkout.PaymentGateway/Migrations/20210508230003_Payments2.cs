using Microsoft.EntityFrameworkCore.Migrations;

namespace Checkout.PaymentGateway.Migrations
{
    public partial class Payments2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CardId",
                table: "Transactions",
                newName: "BankTransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BankTransactionId",
                table: "Transactions",
                newName: "CardId");
        }
    }
}
