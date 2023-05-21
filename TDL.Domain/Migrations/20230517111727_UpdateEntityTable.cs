using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TDL.Domain.Migrations
{
    public partial class UpdateEntityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "tdl_services",
                table: "Workspaces",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "tdl_services",
                table: "Workspaces",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "tdl_services",
                table: "Workspaces",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                schema: "tdl_services",
                table: "Workspaces",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "tdl_services",
                table: "Workspaces",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "tdl_services",
                table: "Workspaces",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "tdl_services",
                table: "Workspaces",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "tdl_services",
                table: "UserWorkspaces",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "tdl_services",
                table: "UserWorkspaces",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "tdl_services",
                table: "UserWorkspaces",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                schema: "tdl_services",
                table: "UserWorkspaces",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "tdl_services",
                table: "UserWorkspaces",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "tdl_services",
                table: "UserWorkspaces",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "tdl_services",
                table: "UserWorkspaces",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "tdl_services",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "tdl_services",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "tdl_services",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "tdl_services",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "tdl_services",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "tdl_services",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "tdl_services",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "tdl_services",
                table: "UserWorkspaces");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "tdl_services",
                table: "UserWorkspaces");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "tdl_services",
                table: "UserWorkspaces");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "tdl_services",
                table: "UserWorkspaces");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "tdl_services",
                table: "UserWorkspaces");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "tdl_services",
                table: "UserWorkspaces");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "tdl_services",
                table: "UserWorkspaces");
        }
    }
}
