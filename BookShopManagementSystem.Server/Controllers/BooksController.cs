using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookShop.Server.Models;

namespace BookShop.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookShopAPIContext _context;

        public BooksController(BookShopAPIContext context)
        {
            _context = context;
        }

        // GET: Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Book
                .Include(b => b.BookCategory)
                .ToListAsync();
        }

        // GET: Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Book
                .Include(b => b.BookCategory)
                .FirstOrDefaultAsync(b => b.BookID == id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // GET: Books/Category/5
        [HttpGet("Category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByCategory(int categoryId)
        {
            return await _context.Book
                .Include(b => b.BookCategory)
                .Where(b => b.BookCategoryID == categoryId)
                .ToListAsync();
        }

        // POST: Books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure BookID is 0 so database generates it
            book.BookID = 0;

            _context.Book.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.BookID }, book);
        }

        // PUT: Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.BookID)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        // DELETE: Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: Books/5/Stock (Update stock only)
        [HttpPatch("{id}/Stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] int stock)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            book.Stock = stock;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.BookID == id);
        }
    }
}