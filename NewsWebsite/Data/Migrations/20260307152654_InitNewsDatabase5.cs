using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebsite.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitNewsDatabase5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-id-001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "93757bfc-dc2b-4730-abec-c22106a8f40a", "AQAAAAIAAYagAAAAEEu4MwnYqmcJtjcKyOWFKHK3JgBh3T3dmKEIwYbQRJapq5SCgBHVOblVMe64FZt4yQ==", "afe9db6c-8cf8-4747-a82b-4857d6aaa6d0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-id-001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "28922d56-e68c-4528-afb0-6e14edfcfc32", "AQAAAAIAAYagAAAAEOZSv62QLAKdxnjDH84LzZbJjL2R2oOQP5Exhwc2C6oCY0zW0ec5+67s7eRaW6evWQ==", "" });
        }
    }
}
