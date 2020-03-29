using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalFinance.Data.Migrations
{
    public partial class Keys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transactions",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3cd25e1b-2501-4fce-8bac-fb7ab724d6a0",
                column: "ConcurrencyStamp",
                value: "011427b5-0d62-42de-86d7-2b92a6500f52");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b122f3c-8700-4208-8902-d77b72eff1b9",
                column: "ConcurrencyStamp",
                value: "f19746b4-f50e-458f-a7de-17cf825614c6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cef3b69a-9c1d-4bbd-8c98-e3024d93c924",
                column: "ConcurrencyStamp",
                value: "5473c2e1-e851-4489-81f3-b82bf356d61b");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 300,
                oldNullable: true);

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
    }
}
