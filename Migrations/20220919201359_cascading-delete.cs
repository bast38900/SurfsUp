using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfsUp.Migrations
{
    public partial class cascadingdelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rent_Board_BoardId",
                table: "Rent");

            migrationBuilder.AlterColumn<Guid>(
                name: "BoardId",
                table: "Rent",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Rent",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Rent_Board_BoardId",
                table: "Rent",
                column: "BoardId",
                principalTable: "Board",
                principalColumn: "BoardId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rent_Board_BoardId",
                table: "Rent");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Rent");

            migrationBuilder.AlterColumn<Guid>(
                name: "BoardId",
                table: "Rent",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Rent_Board_BoardId",
                table: "Rent",
                column: "BoardId",
                principalTable: "Board",
                principalColumn: "BoardId");
        }
    }
}
