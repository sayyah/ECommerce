using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesToProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegisterDate" },
                values: new object[] { "2269e981-ca51-43cf-9413-b191f8df2edc", "AQAAAAIAAYagAAAAEKk4Mdc9L0uxXAF2BXtgqZYqkoQO6SYtC9ZfdaJsTpbldOtFaZCtryPRPG5JZp2e/Q==", new DateTime(2023, 12, 27, 0, 39, 45, 533, DateTimeKind.Local).AddTicks(257) });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Url",
                table: "Products",
                column: "Url",
                unique: true,
                filter: "[Url] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Name",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Url",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegisterDate" },
                values: new object[] { "f94f6004-64c1-4cae-a2f8-9961b732a244", "AQAAAAIAAYagAAAAEJWLIVkoUL06lCtZsO6+uN+GsRtFbMhUxNPspxwEvhnExjH1SGGZ69Ee6h4POHsqzA==", new DateTime(2023, 12, 10, 23, 35, 23, 442, DateTimeKind.Local).AddTicks(4551) });
        }
    }
}
