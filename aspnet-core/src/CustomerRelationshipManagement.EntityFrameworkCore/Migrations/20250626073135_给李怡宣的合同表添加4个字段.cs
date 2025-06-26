using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerRelationshipManagement.Migrations
{
    /// <inheritdoc />
    public partial class 给李怡宣的合同表添加4个字段 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApproveComments",
                table: "App_CrmContract",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ApproveTimes",
                table: "App_CrmContract",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "CurrentStep",
                table: "App_CrmContract",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "App_CrmContract",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveComments",
                table: "App_CrmContract");

            migrationBuilder.DropColumn(
                name: "ApproveTimes",
                table: "App_CrmContract");

            migrationBuilder.DropColumn(
                name: "CurrentStep",
                table: "App_CrmContract");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "App_CrmContract");
        }
    }
}
