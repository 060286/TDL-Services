using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TDL.Domain.Migrations
{
    public partial class ChangeConfigurationOfTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTasks_Todos_TodoId",
                schema: "tdl_services",
                table: "SubTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Todos_TodoId",
                schema: "tdl_services",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Todos_TodoCategories_CategoryId",
                schema: "tdl_services",
                table: "Todos");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkspaces_Users_UserId",
                schema: "tdl_services",
                table: "UserWorkspaces");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkspaces_Workspaces_WorkspaceId",
                schema: "tdl_services",
                table: "UserWorkspaces");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "tdl_services",
                table: "Workspaces",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "tdl_services",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                schema: "tdl_services",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                schema: "tdl_services",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "tdl_services",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "tdl_services",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceId",
                schema: "tdl_services",
                table: "Todos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Todos_WorkspaceId",
                schema: "tdl_services",
                table: "Todos",
                column: "WorkspaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTasks_Todos_TodoId",
                schema: "tdl_services",
                table: "SubTasks",
                column: "TodoId",
                principalSchema: "tdl_services",
                principalTable: "Todos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Todos_TodoId",
                schema: "tdl_services",
                table: "Tags",
                column: "TodoId",
                principalSchema: "tdl_services",
                principalTable: "Todos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TodoCategories_CategoryId",
                schema: "tdl_services",
                table: "Todos",
                column: "CategoryId",
                principalSchema: "tdl_services",
                principalTable: "TodoCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Workspaces_WorkspaceId",
                schema: "tdl_services",
                table: "Todos",
                column: "WorkspaceId",
                principalSchema: "tdl_services",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkspaces_Users_UserId",
                schema: "tdl_services",
                table: "UserWorkspaces",
                column: "UserId",
                principalSchema: "tdl_services",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkspaces_Workspaces_WorkspaceId",
                schema: "tdl_services",
                table: "UserWorkspaces",
                column: "WorkspaceId",
                principalSchema: "tdl_services",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTasks_Todos_TodoId",
                schema: "tdl_services",
                table: "SubTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Todos_TodoId",
                schema: "tdl_services",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Todos_TodoCategories_CategoryId",
                schema: "tdl_services",
                table: "Todos");

            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Workspaces_WorkspaceId",
                schema: "tdl_services",
                table: "Todos");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkspaces_Users_UserId",
                schema: "tdl_services",
                table: "UserWorkspaces");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkspaces_Workspaces_WorkspaceId",
                schema: "tdl_services",
                table: "UserWorkspaces");

            migrationBuilder.DropIndex(
                name: "IX_Todos_WorkspaceId",
                schema: "tdl_services",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                schema: "tdl_services",
                table: "Todos");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "tdl_services",
                table: "Workspaces",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "tdl_services",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                schema: "tdl_services",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                schema: "tdl_services",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "tdl_services",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "tdl_services",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_SubTasks_Todos_TodoId",
                schema: "tdl_services",
                table: "SubTasks",
                column: "TodoId",
                principalSchema: "tdl_services",
                principalTable: "Todos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Todos_TodoId",
                schema: "tdl_services",
                table: "Tags",
                column: "TodoId",
                principalSchema: "tdl_services",
                principalTable: "Todos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TodoCategories_CategoryId",
                schema: "tdl_services",
                table: "Todos",
                column: "CategoryId",
                principalSchema: "tdl_services",
                principalTable: "TodoCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkspaces_Users_UserId",
                schema: "tdl_services",
                table: "UserWorkspaces",
                column: "UserId",
                principalSchema: "tdl_services",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkspaces_Workspaces_WorkspaceId",
                schema: "tdl_services",
                table: "UserWorkspaces",
                column: "WorkspaceId",
                principalSchema: "tdl_services",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
