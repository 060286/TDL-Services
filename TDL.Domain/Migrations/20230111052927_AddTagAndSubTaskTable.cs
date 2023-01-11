using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TDL.Domain.Migrations
{
    public partial class AddTagAndSubTaskTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todo_TodoCategories_ToDoCategoryId",
                schema: "tdl_services",
                table: "Todo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Todo",
                schema: "tdl_services",
                table: "Todo");

            migrationBuilder.DropIndex(
                name: "IX_Todo_ToDoCategoryId",
                schema: "tdl_services",
                table: "Todo");

            migrationBuilder.DropColumn(
                name: "ToDoCategoryId",
                schema: "tdl_services",
                table: "Todo");

            migrationBuilder.RenameTable(
                name: "Todo",
                schema: "tdl_services",
                newName: "Todos",
                newSchema: "tdl_services");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                schema: "tdl_services",
                table: "Todos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                schema: "tdl_services",
                table: "Todos",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchieved",
                schema: "tdl_services",
                table: "Todos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                schema: "tdl_services",
                table: "Todos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RemindedAt",
                schema: "tdl_services",
                table: "Todos",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Todos",
                schema: "tdl_services",
                table: "Todos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SubTasks",
                schema: "tdl_services",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    TodoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubTasks_Todos_TodoId",
                        column: x => x.TodoId,
                        principalSchema: "tdl_services",
                        principalTable: "Todos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "tdl_services",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    Color = table.Column<string>(nullable: false),
                    TodoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Todos_TodoId",
                        column: x => x.TodoId,
                        principalSchema: "tdl_services",
                        principalTable: "Todos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_TodoId",
                schema: "tdl_services",
                table: "SubTasks",
                column: "TodoId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_TodoId",
                schema: "tdl_services",
                table: "Tags",
                column: "TodoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubTasks",
                schema: "tdl_services");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "tdl_services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Todos",
                schema: "tdl_services",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                schema: "tdl_services",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "FileName",
                schema: "tdl_services",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "IsArchieved",
                schema: "tdl_services",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                schema: "tdl_services",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "RemindedAt",
                schema: "tdl_services",
                table: "Todos");

            migrationBuilder.RenameTable(
                name: "Todos",
                schema: "tdl_services",
                newName: "Todo",
                newSchema: "tdl_services");

            migrationBuilder.AddColumn<Guid>(
                name: "ToDoCategoryId",
                schema: "tdl_services",
                table: "Todo",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Todo",
                schema: "tdl_services",
                table: "Todo",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Todo_ToDoCategoryId",
                schema: "tdl_services",
                table: "Todo",
                column: "ToDoCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todo_TodoCategories_ToDoCategoryId",
                schema: "tdl_services",
                table: "Todo",
                column: "ToDoCategoryId",
                principalSchema: "tdl_services",
                principalTable: "TodoCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
