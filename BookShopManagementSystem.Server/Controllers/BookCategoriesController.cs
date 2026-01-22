using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookShop.Server.Models;

namespace BookShop.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookCategoriesController : ControllerBase
    {
        private readonly BookShopAPIContext _context;

        public BookCategoriesController(BookShopAPIContext context)
        {
            _context = context;
        }

        // GET: BookCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookCategory>>> GetBookCategory()
        {
            return await _context.BookCategory
                .Include(bc => bc.Books)
                .ToListAsync();
        }

        // GET: BookCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookCategory>> GetBookCategory(int id)
        {
            var bookCategory = await _context.BookCategory
                .Include(bc => bc.Books)
                .FirstOrDefaultAsync(bc => bc.BookCategoryID == id);

            if (bookCategory == null)
            {
                return NotFound();
            }

            return bookCategory;
        }

        // PUT: BookCategories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookCategory(int id, BookCategory bookCategory)
        {
            if (id != bookCategory.BookCategoryID)
            {
                return BadRequest("ID mismatch");
            }

            // Load existing category with its books
            var existingCategory = await _context.BookCategory
                .Include(bc => bc.Books)
                .FirstOrDefaultAsync(bc => bc.BookCategoryID == id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            // Update category name
            existingCategory.Name = bookCategory.Name;

            // Remove all existing books for this category
            if (existingCategory.Books != null && existingCategory.Books.Any())
            {
                _context.Book.RemoveRange(existingCategory.Books);
            }

            // Add new books from the request
            if (bookCategory.Books != null && bookCategory.Books.Any())
            {
                foreach (var book in bookCategory.Books)
                {
                    // Ensure the foreign key is set correctly
                    book.BookCategoryID = id;
                    book.BookID = 0; // Let database generate new ID
                    _context.Book.Add(book);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookCategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: BookCategories
        [HttpPost]
        public async Task<ActionResult<BookCategory>> PostBookCategory(BookCategory bookCategory)
        {
            // Ensure BookCategoryID is 0 so database generates it
            bookCategory.BookCategoryID = 0;

            // If books are included, set their IDs to 0 and ensure foreign key
            if (bookCategory.Books != null && bookCategory.Books.Any())
            {
                foreach (var book in bookCategory.Books)
                {
                    book.BookID = 0; // Let database generate ID
                    book.BookCategoryID = 0; // Will be set after category is saved
                }
            }

            _context.BookCategory.Add(bookCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookCategory", new { id = bookCategory.BookCategoryID }, bookCategory);
        }

        // DELETE: BookCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookCategory(int id)
        {
            // Load category with books
            var bookCategory = await _context.BookCategory
                .Include(bc => bc.Books)
                .FirstOrDefaultAsync(bc => bc.BookCategoryID == id);

            if (bookCategory == null)
            {
                return NotFound();
            }

            // Delete all books in this category first
            if (bookCategory.Books != null && bookCategory.Books.Any())
            {
                _context.Book.RemoveRange(bookCategory.Books);
            }

            // Then delete the category
            _context.BookCategory.Remove(bookCategory);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookCategoryExists(int id)
        {
            return _context.BookCategory.Any(e => e.BookCategoryID == id);
        }
    }
}