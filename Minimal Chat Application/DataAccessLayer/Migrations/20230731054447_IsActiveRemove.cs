using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Minimal_Chat_Application.Migrations
{
    /// <inheritdoc />
    public partial class IsActiveRemove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Messages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Messages",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
