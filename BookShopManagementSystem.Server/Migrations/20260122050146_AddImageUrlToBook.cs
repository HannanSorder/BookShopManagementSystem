using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShopManagementSystem.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Book",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                });

            migrationBuilder.UpdateData(
                table: "Book",
                keyColumn: "BookID",
                keyValue: 1,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "Book",
                keyColumn: "BookID",
                keyValue: 2,
                column: "ImageUrl",
                value: "/images/books/mockingbird.jpg");

            migrationBuilder.UpdateData(
                table: "Book",
                keyColumn: "BookID",
                keyValue: 3,
                column: "ImageUrl",
                value: "/images/books/sapiens.jpg");

            migrationBuilder.UpdateData(
                table: "Book",
                keyColumn: "BookID",
                keyValue: 4,
                column: "ImageUrl",
                value: "/images/books/brief-history.jpg");

            migrationBuilder.UpdateData(
                table: "Book",
                keyColumn: "BookID",
                keyValue: 5,
                column: "ImageUrl",
                value: "/images/books/bangladesh-history.jpg");

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserID", "CreatedAt", "Email", "Password", "Role", "Username" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@bookshop.com", "admin123", "Admin", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Book");
        }
    }
}
