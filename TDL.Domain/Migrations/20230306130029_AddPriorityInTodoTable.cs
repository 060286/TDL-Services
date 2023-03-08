using Microsoft.EntityFrameworkCore.Migrations;

namespace TDL.Domain.Migrations
{
    public partial class AddPriorityInTodoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Priority",
                schema: "tdl_services",
                table: "Todos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                schema: "tdl_services",
                table: "Todos");
        }
    }
}
