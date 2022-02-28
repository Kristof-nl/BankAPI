using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddedBankTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_ContactInfo_ContactInfoId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_BankTransaction_Banks_BankId",
                table: "BankTransaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactInfo",
                table: "ContactInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankTransaction",
                table: "BankTransaction");

            migrationBuilder.RenameTable(
                name: "ContactInfo",
                newName: "ContactInfos");

            migrationBuilder.RenameTable(
                name: "BankTransaction",
                newName: "BankTransactions");

            migrationBuilder.RenameIndex(
                name: "IX_BankTransaction_BankId",
                table: "BankTransactions",
                newName: "IX_BankTransactions_BankId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactInfos",
                table: "ContactInfos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankTransactions",
                table: "BankTransactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_ContactInfos_ContactInfoId",
                table: "Addresses",
                column: "ContactInfoId",
                principalTable: "ContactInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_Banks_BankId",
                table: "BankTransactions",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_ContactInfos_ContactInfoId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_Banks_BankId",
                table: "BankTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactInfos",
                table: "ContactInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankTransactions",
                table: "BankTransactions");

            migrationBuilder.RenameTable(
                name: "ContactInfos",
                newName: "ContactInfo");

            migrationBuilder.RenameTable(
                name: "BankTransactions",
                newName: "BankTransaction");

            migrationBuilder.RenameIndex(
                name: "IX_BankTransactions_BankId",
                table: "BankTransaction",
                newName: "IX_BankTransaction_BankId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactInfo",
                table: "ContactInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankTransaction",
                table: "BankTransaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_ContactInfo_ContactInfoId",
                table: "Addresses",
                column: "ContactInfoId",
                principalTable: "ContactInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransaction_Banks_BankId",
                table: "BankTransaction",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
