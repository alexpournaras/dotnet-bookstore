using Microsoft.AspNetCore.Mvc;
using BookstoreAPI.Models;
using BookstoreAPI.Filters;
using BookstoreAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace BookstoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // [Authorize(Roles = "Developer,User")]
        [HttpGet]
        public ActionResult<List<Book>> GetAllBooks()
        {
            try
            {
                var books = _bookService.GetAll();
                return Ok(books);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(int id)
        {
            try
            {
                var book = _bookService.Get(id);
                if (book == null)
                {
                    return NotFound(new { message = "Book not found!" });
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [IPLocationLookup]
        [HttpPost]
        public ActionResult<Book> CreateBook(Book book)
        {
            try
            {
                var res = _bookService.Add(book);
                if (res == null)
                {
                    return BadRequest(new { message = "Cannot insert book. Author not found!" });
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch]
        public ActionResult<Book> UpdateBook(UpdateBookEntity book)
        {
            try
            {
                var res = _bookService.Update(book);
                if (res == 0)
                {
                    return NotFound(new { message = "Book not found!" });
                }
                else if (res == -1)
                {
                    return BadRequest(new { message = "Cannot update book. Author not found!" });
                }

                return Ok(new { rows_affected = res });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [IPLocationLookup]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var res = _bookService.Delete(id);
                if (res == 0)
                {
                    return NotFound(new { message = "Book not found!" });
                }

                return Ok(new { rows_affected = res });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("search")]
        // [Authorize(Roles = "Developer,User")]
        public ActionResult<List<Book>> SearchBooks(string searchTerm)
        {
            try
            {
                var books = _bookService.SearchBooks(searchTerm);
                return Ok(books);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
