using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalFinance.Data.Migrations
{
    public partial class TransDate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3cd25e1b-2501-4fce-8bac-fb7ab724d6a0",
                column: "ConcurrencyStamp",
                value: "3cd85ad9-c6ef-47a7-846c-bf331cbf7716");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b122f3c-8700-4208-8902-d77b72eff1b9",
                column: "ConcurrencyStamp",
                value: "8a2eeb36-f2ff-4326-a4b8-8e564de950fb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cef3b69a-9c1d-4bbd-8c98-e3024d93c924",
                column: "ConcurrencyStamp",
                value: "36deb993-58d1-4986-ad81-3b1846200d15");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3cd25e1b-2501-4fce-8bac-fb7ab724d6a0",
                column: "ConcurrencyStamp",
                value: "158a2b7c-7779-4326-a5dc-0f03426fd0dd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b122f3c-8700-4208-8902-d77b72eff1b9",
                column: "ConcurrencyStamp",
                value: "a7f4e992-c17d-4146-a63c-fd122b1839ba");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cef3b69a-9c1d-4bbd-8c98-e3024d93c924",
                column: "ConcurrencyStamp",
                value: "d1306c5f-a847-471c-ab8e-1f333b3594cd");
        }
    }
}
