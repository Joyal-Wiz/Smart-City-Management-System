using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartCity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationUniqueKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqueKey",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueKey",
                table: "Notifications");
        }
    }
}
