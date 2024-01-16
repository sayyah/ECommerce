using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class removerequiredattributfromCodeinDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegisterDate" },
                values: new object[] { "3a4fb53b-31d1-4fd1-9bd6-db6d78e4ca6a", "AQAAAAIAAYagAAAAEGoqvyXXS1U9KCLJrSWdDU6/z1BHiG6lGaAf3lUq05s2WVnCfuuKkqYW5kwz3WbZEw==", new DateTime(2024, 1, 18, 9, 25, 58, 215, DateTimeKind.Local).AddTicks(1111) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegisterDate" },
                values: new object[] { "5cd1d907-7698-40e2-8516-e7fc4aea20f6", "AQAAAAIAAYagAAAAEMH5lh4+DdnbK5Ggk1XIuClDSu/kC4QLnpwBuJBHEfCJbDn+u9dzTqEZQFBIHGHbTQ==", new DateTime(2024, 1, 16, 16, 21, 32, 736, DateTimeKind.Local).AddTicks(3523) });
        }
    }
}
