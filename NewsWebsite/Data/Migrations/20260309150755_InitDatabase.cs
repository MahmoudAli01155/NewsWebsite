using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebsite.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-id-001",
                columns: new[] { "ConcurrencyStamp", "FullName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9060f610-fa11-471b-ae70-c1c4c2555d43", "Administrator", "AQAAAAIAAYagAAAAEPkrqPopx5O9zNGSusGsiwZowd6PD/GUo0oOY63K283/TOx1ZhpJdQspvzlH0e0eXw==", "554275c9-4a1a-49a4-bef8-bebdc458a30d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-id-001");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "admin-id-001", 0, "93757bfc-dc2b-4730-abec-c22106a8f40a", "IdentityUser", "admin@example.com", true, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEEu4MwnYqmcJtjcKyOWFKHK3JgBh3T3dmKEIwYbQRJapq5SCgBHVOblVMe64FZt4yQ==", null, false, "afe9db6c-8cf8-4747-a82b-4857d6aaa6d0", false, "admin" });
        }
    }
}
