using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerRelationshipManagement.Migrations
{
    /// <inheritdoc />
    public partial class 修改刘畅的表4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "App_UserInfo",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.DropColumn(
                name: "test",
                table: "App_Invoice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "test",
                table: "App_Invoice",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "App_UserInfo",
                columns: new[] { "Id", "Avatar", "ConcurrencyStamp", "CreationTime", "CreatorId", "DeleterId", "DeletionTime", "Email", "ExtraProperties", "IsActive", "LastModificationTime", "LastModifierId", "Password", "PhoneInfo", "RealName", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000000"), "", "eb2d3da752674a65be6e8f5d3b5a0835", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "admin@admin.com", "{}", true, null, null, "123", "12345678901", "", "admin" });
        }
    }
}
