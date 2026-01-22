using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Server.Models
{
    [Table(nameof(Book))]
    public class Book
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }

        public string Publisher { get; set; }

        [Required]
        public decimal PurchasePrice { get; set; }

        [Required]
        public decimal SellingPrice { get; set; }

        [Required]
        public int Pages { get; set; }

        [Required]
        public int Stock { get; set; }

        // 🆕 NEW: Image URL field added
        public string? ImageUrl { get; set; }

        [ForeignKey(nameof(BookCategory.BookCategoryID))]
        public int BookCategoryID { get; set; }
        public BookCategory? BookCategory { get; set; }
    }

    [Table(nameof(BookCategory))]
    public class BookCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookCategoryID { get; set; }

        [Required]
        public string Name { get; set; }
        public ICollection<Book>? Books { get; set; }
    }

    // 🆕 User Model
    [Table(nameof(User))]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // ✅ Default value
    }

    public class BookShopAPIContext : DbContext
    {
        public BookShopAPIContext(DbContextOptions<BookShopAPIContext> options)
            : base(options)
        {
        }

        public DbSet<BookCategory> BookCategory { get; set; } = default!;
        public DbSet<Book> Book { get; set; } = default!;
        public DbSet<User> User { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookCategory>().HasData(new BookCategory[]
            {
                new BookCategory { BookCategoryID = 1, Name = "Fiction" },
                new BookCategory { BookCategoryID = 2, Name = "Non-Fiction" },
                new BookCategory { BookCategoryID = 3, Name = "Science" },
                new BookCategory { BookCategoryID = 4, Name = "History" },
            });

            modelBuilder.Entity<Book>().HasData(new Book[]
            {
                //new Book
                //{
                //    BookID = 1,
                //    Title = "The Great Gatsby",
                //    ISBN = "978-0743273565",
                //    Author = "F. Scott Fitzgerald",
                //    Publisher = "Scribner",
                //    PurchasePrice = 250.00M,
                //    SellingPrice = 450.00M,
                //    Pages = 180,
                //    Stock = 50,
                //    BookCategoryID = 1,
                //    ImageUrl = null // ✅ Changed from "" to null
                //},
                //new Book
                //{
                //    BookID = 2,
                //    Title = "To Kill a Mockingbird",
                //    ISBN = "978-0061120084",
                //    Author = "Harper Lee",
                //    Publisher = "Harper Perennial",
                //    PurchasePrice = 300.00M,
                //    SellingPrice = 500.00M,
                //    Pages = 324,
                //    Stock = 40,
                //    BookCategoryID = 1,
                //    ImageUrl = null
                //},
                //new Book
                //{
                //    BookID = 3,
                //    Title = "Sapiens",
                //    ISBN = "978-0062316110",
                //    Author = "Yuval Noah Harari",
                //    Publisher = "Harper",
                //    PurchasePrice = 400.00M,
                //    SellingPrice = 650.00M,
                //    Pages = 443,
                //    Stock = 30,
                //    BookCategoryID = 2,
                //    ImageUrl = null
                //},
                //new Book
                //{
                //    BookID = 4,
                //    Title = "A Brief History of Time",
                //    ISBN = "978-0553380163",
                //    Author = "Stephen Hawking",
                //    Publisher = "Bantam",
                //    PurchasePrice = 350.00M,
                //    SellingPrice = 550.00M,
                //    Pages = 256,
                //    Stock = 25,
                //    BookCategoryID = 3,
                //    ImageUrl = null
                //},
                //new Book
                //{
                //    BookID = 5,
                //    Title = "The History of Bangladesh",
                //    ISBN = "978-9840515752",
                //    Author = "Willem van Schendel",
                //    Publisher = "Cambridge University Press",
                //    PurchasePrice = 500.00M,
                //    SellingPrice = 800.00M,
                //    Pages = 392,
                //    Stock = 20,
                //    BookCategoryID = 4,
                //    ImageUrl = null
                //}
            });

            // Admin User Seed
            modelBuilder.Entity<User>().HasData(new User
            {
                UserID = 1,
                Username = "admin",
                Password = "admin123",
                Email = "admin@bookshop.com",
                Role = "Admin",
                CreatedAt = DateTime.UtcNow // ✅ Added
            });
        }
    }
}
