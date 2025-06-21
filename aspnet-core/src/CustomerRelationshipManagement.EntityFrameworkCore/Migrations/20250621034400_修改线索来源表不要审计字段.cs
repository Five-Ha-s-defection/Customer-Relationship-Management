using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerRelationshipManagement.Migrations
{
    /// <inheritdoc />
    public partial class 修改线索来源表不要审计字段 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "ClueSource");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "ClueSource");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "ClueSource");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "ClueSource");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "ClueSource");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "ClueSource");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ClueSource");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "ClueSource");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "ClueSource");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "ClueSource",
                type: "varchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "ClueSource",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "ClueSource",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "ClueSource",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "ClueSource",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "ClueSource",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ClueSource",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "ClueSource",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "ClueSource",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");
        }
    }
}
