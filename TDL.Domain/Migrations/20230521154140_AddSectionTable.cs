using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TDL.Domain.Migrations
{
    public partial class AddSectionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SectionId",
                schema: "tdl_services",
                table: "Todos",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sections",
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
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    WorkspaceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalSchema: "tdl_services",
                        principalTable: "Workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Todos_SectionId",
                schema: "tdl_services",
                table: "Todos",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_WorkspaceId",
                schema: "tdl_services",
                table: "Sections",
                column: "WorkspaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Sections_SectionId",
                schema: "tdl_services",
                table: "Todos",
                column: "SectionId",
                principalSchema: "tdl_services",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Sections_SectionId",
                schema: "tdl_services",
                table: "Todos");

            migrationBuilder.DropTable(
                name: "Sections",
                schema: "tdl_services");

            migrationBuilder.DropIndex(
                name: "IX_Todos_SectionId",
                schema: "tdl_services",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "SectionId",
                schema: "tdl_services",
                table: "Todos");
        }
    }
}
