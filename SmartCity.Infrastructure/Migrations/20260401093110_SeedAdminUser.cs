using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartCity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsBlocked", "Name", "PasswordHash", "PhoneNumber", "Role" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "admin@smartcity.com", false, "Admin", "6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=", "9999999999", 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));
        }
    }
}
