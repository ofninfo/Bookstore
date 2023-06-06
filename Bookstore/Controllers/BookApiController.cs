using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bookstore.Data;
using Bookstore.Models;

namespace Bookstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BookApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookModel>>> GetBooks()
        {
          if (_context.Books == null)
          {
              return NotFound();
          }
            return await _context.Books.ToListAsync();
        }

        // GET: api/BookApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookModel>> GetBook(int id)
        {
          if (_context.Books == null)
          {
              return NotFound();
          }
            var bookModel = await _context.Books.FindAsync(id);

            if (bookModel == null)
            {
                return NotFound();
            }

            return bookModel;
        }

        // PUT: api/BookApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookModel(int id, BookModel bookModel)
        {
            if (id != bookModel.BookId)
            {
                return BadRequest();
            }

            _context.Entry(bookModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookModelExists(id))
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

        // POST: api/BookApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookModel>> PostBookModel(BookModel bookModel)
        {
          if (_context.Books == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Books'  is null.");
          }
            _context.Books.Add(bookModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookModel", new { id = bookModel.BookId }, bookModel);
        }

        // DELETE: api/BookApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookModel(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var bookModel = await _context.Books.FindAsync(id);
            if (bookModel == null)
            {
                return NotFound();
            }

            _context.Books.Remove(bookModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookModelExists(int id)
        {
            return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
    }
}
