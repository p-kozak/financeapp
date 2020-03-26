using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalFinance.Data.Migrations
{
    public partial class TransDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0b394f50-3718-4987-9377-5f3223e8355b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "20f8cc37-86b1-4391-b5e6-264f08edd6be");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "57c3301d-4c6d-4fd8-833c-633a29d57834");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cef3b69a-9c1d-4bbd-8c98-e3024d93c924", "d1306c5f-a847-471c-ab8e-1f333b3594cd", "Visitor", "VISITOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6b122f3c-8700-4208-8902-d77b72eff1b9", "a7f4e992-c17d-4146-a63c-fd122b1839ba", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3cd25e1b-2501-4fce-8bac-fb7ab724d6a0", "158a2b7c-7779-4326-a5dc-0f03426fd0dd", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3cd25e1b-2501-4fce-8bac-fb7ab724d6a0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b122f3c-8700-4208-8902-d77b72eff1b9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cef3b69a-9c1d-4bbd-8c98-e3024d93c924");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Transactions");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0b394f50-3718-4987-9377-5f3223e8355b", "01ed6d77-d782-43d4-a69e-1203ae9f4b8e", "Visitor", "VISITOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "20f8cc37-86b1-4391-b5e6-264f08edd6be", "1fa4dab1-f21a-49a5-ae48-b8ab7acb4633", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "57c3301d-4c6d-4fd8-833c-633a29d57834", "d5d74148-8721-4c9a-bc5c-035abb69ee46", "Admin", "ADMIN" });
        }
    }
}
