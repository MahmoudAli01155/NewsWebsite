using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebsite.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitNewsDatabase4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-id-001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "28922d56-e68c-4528-afb0-6e14edfcfc32", "AQAAAAIAAYagAAAAEOZSv62QLAKdxnjDH84LzZbJjL2R2oOQP5Exhwc2C6oCY0zW0ec5+67s7eRaW6evWQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-id-001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "733c70e4-4600-48d6-83b1-d8c6aef516a3", "AQAAAAIAAYagAAAAENXxucgkmJbx6K/I6QnfB4w7V393vv9IIRmqtKJs0eyMSaylKZCZxCmih+h19/0XaA==" });
        }
    }
}
