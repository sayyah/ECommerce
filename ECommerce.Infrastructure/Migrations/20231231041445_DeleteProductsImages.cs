using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class DeleteProductsImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_HolooCompanies_HolooCompanyId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Products_ProductId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_HolooCompanies_HolooCompanyId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_HolooCompanies_HolooCompanyId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_HolooCompanies_HolooCompanyId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_HolooCompanies_HolooCompanyId",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HolooCompanies",
                table: "HolooCompanies");

            migrationBuilder.RenameTable(
                name: "HolooCompanies",
                newName: "HolooCompany");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HolooCompany",
                table: "HolooCompany",
                column: "Id");

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegisterDate" },
                values: new object[] { "6059c33c-aacc-4348-a731-ce1727c331f6", "AQAAAAIAAYagAAAAEK8wjIF038UjDcsPXFdGExtZQBSLTkOtEMf2hQUMkHV7YFtFDyEFT24PfMD15e7lhg==", new DateTime(2023, 12, 31, 7, 44, 44, 95, DateTimeKind.Local).AddTicks(771) });

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_HolooCompany_HolooCompanyId",
                table: "Employees",
                column: "HolooCompanyId",
                principalTable: "HolooCompany",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Products_ProductId",
                table: "Images",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_HolooCompany_HolooCompanyId",
                table: "Products",
                column: "HolooCompanyId",
                principalTable: "HolooCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_HolooCompany_HolooCompanyId",
                table: "Transactions",
                column: "HolooCompanyId",
                principalTable: "HolooCompany",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_HolooCompany_HolooCompanyId",
                table: "Units",
                column: "HolooCompanyId",
                principalTable: "HolooCompany",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_HolooCompany_HolooCompanyId",
                schema: "Security",
                table: "Users",
                column: "HolooCompanyId",
                principalTable: "HolooCompany",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_HolooCompany_HolooCompanyId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Products_ProductId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_HolooCompany_HolooCompanyId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_HolooCompany_HolooCompanyId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_HolooCompany_HolooCompanyId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_HolooCompany_HolooCompanyId",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HolooCompany",
                table: "HolooCompany");

            migrationBuilder.RenameTable(
                name: "HolooCompany",
                newName: "HolooCompanies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HolooCompanies",
                table: "HolooCompanies",
                column: "Id");

            migrationBuilder.UpdateData(
                schema: "Security",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegisterDate" },
                values: new object[] { "f94f6004-64c1-4cae-a2f8-9961b732a244", "AQAAAAIAAYagAAAAEJWLIVkoUL06lCtZsO6+uN+GsRtFbMhUxNPspxwEvhnExjH1SGGZ69Ee6h4POHsqzA==", new DateTime(2023, 12, 10, 23, 35, 23, 442, DateTimeKind.Local).AddTicks(4551) });

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_HolooCompanies_HolooCompanyId",
                table: "Employees",
                column: "HolooCompanyId",
                principalTable: "HolooCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Products_ProductId",
                table: "Images",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_HolooCompanies_HolooCompanyId",
                table: "Products",
                column: "HolooCompanyId",
                principalTable: "HolooCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_HolooCompanies_HolooCompanyId",
                table: "Transactions",
                column: "HolooCompanyId",
                principalTable: "HolooCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_HolooCompanies_HolooCompanyId",
                table: "Units",
                column: "HolooCompanyId",
                principalTable: "HolooCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_HolooCompanies_HolooCompanyId",
                schema: "Security",
                table: "Users",
                column: "HolooCompanyId",
                principalTable: "HolooCompanies",
                principalColumn: "Id");
        }
    }
}
