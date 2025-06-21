using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_commerce.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDataInUserRolesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0087edcf-fb42-45fb-91f6-80ae9b71de95", null, "Admin", "ADMIN" },
                    { "22636378-9eb7-42a3-a042-e811dfaf65c2", null, "Customer", "CUSTOMER" },
                    { "b10c2c06-a285-4048-bbe7-3a05bb192029", null, "Company", "COMPANY" },
                    { "ebec6673-cb71-424a-a475-ea99191e7f2d", null, "Employee", "EMPLOYEE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "0087edcf-fb42-45fb-91f6-80ae9b71de95");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "22636378-9eb7-42a3-a042-e811dfaf65c2");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "b10c2c06-a285-4048-bbe7-3a05bb192029");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "ebec6673-cb71-424a-a475-ea99191e7f2d");
        }
    }
}
