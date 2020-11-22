using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace pd_api.Migrations
{
    public partial class add_email_configuration_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "AspNetRoles");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UserCreateId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserModifyId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EmailConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FriendlyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UseDefaultCredential = table.Column<bool>(type: "bit", nullable: false),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Port = table.Column<int>(type: "int", nullable: false),
                    EnableSSL = table.Column<bool>(type: "bit", nullable: false),
                    DefaultMessageBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserCreateId = table.Column<int>(type: "int", nullable: false),
                    UserModifyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailConfigurations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailConfigurations");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ModifyDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserCreateId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserModifyId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
