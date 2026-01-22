using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookShopManagementSystem.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitalCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookCategory",
                columns: table => new
                {
                    BookCategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCategory", x => x.BookCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    BookID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Pages = table.Column<int>(type: "int", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    BookCategoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.BookID);
                    table.ForeignKey(
                        name: "FK_Book_BookCategory_BookCategoryID",
                        column: x => x.BookCategoryID,
                        principalTable: "BookCategory",
                        principalColumn: "BookCategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BookCategory",
                columns: new[] { "BookCategoryID", "Name" },
                values: new object[,]
                {
                    { 1, "Fiction" },
                    { 2, "Non-Fiction" },
                    { 3, "Science" },
                    { 4, "History" }
                });

            migrationBuilder.InsertData(
                table: "Book",
                columns: new[] { "BookID", "Author", "BookCategoryID", "ISBN", "Pages", "Publisher", "PurchasePrice", "SellingPrice", "Stock", "Title" },
                values: new object[,]
                {
                    { 1, "F. Scott Fitzgerald", 1, "978-0743273565", 180, "Scribner", 250.00m, 450.00m, 50, "The Great Gatsby" },
                    { 2, "Harper Lee", 1, "978-0061120084", 324, "Harper Perennial", 300.00m, 500.00m, 40, "To Kill a Mockingbird" },
                    { 3, "Yuval Noah Harari", 2, "978-0062316110", 443, "Harper", 400.00m, 650.00m, 30, "Sapiens" },
                    { 4, "Stephen Hawking", 3, "978-0553380163", 256, "Bantam", 350.00m, 550.00m, 25, "A Brief History of Time" },
                    { 5, "Willem van Schendel", 4, "978-9840515752", 392, "Cambridge University Press", 500.00m, 800.00m, 20, "The History of Bangladesh" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_BookCategoryID",
                table: "Book",
                column: "BookCategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "BookCategory");
        }
    }
}
