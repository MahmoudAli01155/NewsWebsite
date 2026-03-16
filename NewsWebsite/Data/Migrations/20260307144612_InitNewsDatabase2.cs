using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebsite.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitNewsDatabase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-id-001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3aa04e48-e312-4364-94e4-319e4283446f", "AQAAAAIAAYagAAAAEJtfsUS4ciqBW8ZFiOvyAKUB/NAo3iuYjckDuV0ofzMSu7DWmVfUU6qn9vMPRKNhPg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-id-001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "37282b52-ef87-49ae-b7ef-d92eabf14e74", "AQAAAAIAAYagAAAAEM2pUooIqjcwEGdexacv36WQzYWtgoH6TqaPlpMH5gR7N/R2Ry2LBjpTChf4yLes9g==" });
        }
    }
}
