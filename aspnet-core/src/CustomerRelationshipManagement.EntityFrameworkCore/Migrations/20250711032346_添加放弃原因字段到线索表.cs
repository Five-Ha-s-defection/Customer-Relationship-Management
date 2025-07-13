using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerRelationshipManagement.Migrations
{
    /// <inheritdoc />
    public partial class 添加放弃原因字段到线索表 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AbandonReason",
                table: "App_Clue",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AbandonReason",
                table: "App_Clue");
        }
    }
}
