using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerRelationshipManagement.Migrations
{
    /// <inheritdoc />
    public partial class 数据播种 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "App_UserInfo",
                columns: new[] { "Id", "ConcurrencyStamp", "CreationTime", "CreatorId", "DeleterId", "DeletionTime", "Email", "ExtraProperties", "IsActive", "LastModificationTime", "LastModifierId", "Password", "PhoneInfo", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000000"), "b2d6ade421f642c4a81d25e64cadbf9e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "admin@admin.com", "{}", true, null, null, "123", "12345678901", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "App_UserInfo",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
