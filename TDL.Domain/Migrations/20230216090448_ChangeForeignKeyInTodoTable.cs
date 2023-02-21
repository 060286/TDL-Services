using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TDL.Domain.Migrations
{
    public partial class ChangeForeignKeyInTodoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                schema: "tdl_services",
                table: "Todos");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                schema: "tdl_services",
                table: "Todos",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Todos_CategoryId",
                schema: "tdl_services",
                table: "Todos",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TodoCategories_CategoryId",
                schema: "tdl_services",
                table: "Todos",
                column: "CategoryId",
                principalSchema: "tdl_services",
                principalTable: "TodoCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_TodoCategories_CategoryId",
                schema: "tdl_services",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_CategoryId",
                schema: "tdl_services",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "tdl_services",
                table: "Todos");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                schema: "tdl_services",
                table: "Todos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
