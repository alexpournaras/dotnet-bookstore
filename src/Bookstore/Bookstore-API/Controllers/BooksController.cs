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
                return Ok(book);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public ActionResult<Book> CreateBook(Book book)
        {
            try
            {
                _bookService.Add(book);
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
                return Ok("{ rows_affected: " + res + " }");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var res = _bookService.Delete(id);
                return Ok("{ rows_affected: " + res + " }");
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
