using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerRelationshipManagement.Migrations
{
    /// <inheritdoc />
    public partial class 添加字段是否主联系人到联系人表 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "App_CustomerContact",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ContactId",
                table: "App_Customer",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "App_CustomerContact");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "App_Customer");
        }
    }
}
