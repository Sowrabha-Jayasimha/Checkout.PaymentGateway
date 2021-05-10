using Microsoft.EntityFrameworkCore.Migrations;

namespace Checkout.PaymentGateway.Migrations
{
    public partial class Payments3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Cards_CardsCardId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "CardsCardId",
                table: "Transactions",
                newName: "CardId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_CardsCardId",
                table: "Transactions",
                newName: "IX_Transactions_CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Cards_CardId",
                table: "Transactions",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "CardId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Cards_CardId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "CardId",
                table: "Transactions",
                newName: "CardsCardId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_CardId",
                table: "Transactions",
                newName: "IX_Transactions_CardsCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Cards_CardsCardId",
                table: "Transactions",
                column: "CardsCardId",
                principalTable: "Cards",
                principalColumn: "CardId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
