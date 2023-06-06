using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.Data.Migrations
{
    public partial class E : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Client_ClientId",
                table: "Sale");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Sale",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Client_ClientId",
                table: "Sale",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Client_ClientId",
                table: "Sale");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Sale",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Client_ClientId",
                table: "Sale",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "ClientId");
        }
    }
}
