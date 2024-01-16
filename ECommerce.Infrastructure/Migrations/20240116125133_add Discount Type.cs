using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class addDiscountType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CouponQty",
                table: "Discounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                table: "Discounts",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Discounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CouponQty", "DiscountType" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegisterDate" },
                values: new object[] { "5cd1d907-7698-40e2-8516-e7fc4aea20f6", "AQAAAAIAAYagAAAAEMH5lh4+DdnbK5Ggk1XIuClDSu/kC4QLnpwBuJBHEfCJbDn+u9dzTqEZQFBIHGHbTQ==", new DateTime(2024, 1, 16, 16, 21, 32, 736, DateTimeKind.Local).AddTicks(3523) });

            migrationBuilder.CreateIndex(
                name: "IX_Products_DiscountId",
                table: "Products",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Discounts_DiscountId",
                table: "Products",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Discounts_DiscountId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_DiscountId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CouponQty",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "Discounts");

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
