using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerRelationshipManagement.Migrations
{
    /// <inheritdoc />
    public partial class 添加客户分配领取状态字段到客户表 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerPoolStatus",
                table: "App_Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerPoolStatus",
                table: "App_Customer");
        }
    }
}
