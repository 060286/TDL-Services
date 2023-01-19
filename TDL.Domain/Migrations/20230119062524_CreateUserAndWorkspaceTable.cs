using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TDL.Domain.Migrations
{
    public partial class CreateUserAndWorkspaceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
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
                    Img = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workspaces",
                schema: "tdl_services",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserWorkspaces",
                schema: "tdl_services",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    WorkspaceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWorkspaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWorkspaces_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "tdl_services",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWorkspaces_Workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalSchema: "tdl_services",
                        principalTable: "Workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkspaces_UserId",
                schema: "tdl_services",
                table: "UserWorkspaces",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkspaces_WorkspaceId",
                schema: "tdl_services",
                table: "UserWorkspaces",
                column: "WorkspaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserWorkspaces",
                schema: "tdl_services");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "tdl_services");

            migrationBuilder.DropTable(
                name: "Workspaces",
                schema: "tdl_services");
        }
    }
}
