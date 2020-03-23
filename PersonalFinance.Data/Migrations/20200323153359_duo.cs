using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalFinance.Data.Migrations
{
    public partial class duo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "243de973-ae47-405e-a3ee-32a679b58b0c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "260f4aeb-6b26-448c-9763-11f3f929ce34");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a29c599e-6c1a-4dbb-8472-0bed698112fc");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a29c599e-6c1a-4dbb-8472-0bed698112fc", "3c9c7200-0b40-433d-ba67-6283b4a3d83e", "Visitor", "VISITOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "260f4aeb-6b26-448c-9763-11f3f929ce34", "12b01b71-2cfe-45aa-bf2f-6c80c7ef092b", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "243de973-ae47-405e-a3ee-32a679b58b0c", "56ea971e-fdcd-4bf5-aba2-69f0ef39ca96", "Admin", "ADMIN" });
        }
    }
}
