using Microsoft.AspNetCore.Mvc;
using WebAPI.Model;
using WebAPI.Services;
using System.Linq;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private ILibraryService _libraryService;

        public BooksController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpGet]
        public ActionResult<List<Book>> GetAllBooks()
        {
            return _libraryService.GetAllBooks();
        }

        [HttpGet("init/{numberOfBooks}")]
        public ActionResult<List<Book>> InitializeBooks(int numberOfBooks)
        {
            List<Author> authors = _libraryService.GetAllAuthors();
            if (authors.Count == 0)
            {
                return BadRequest($"You cannot initialize books without authors. First add some authors!");
            }

            _libraryService.InitializeBooks(numberOfBooks, authors);

            return _libraryService.GetAllBooks();
        }

        [HttpGet("search")]
        public ActionResult<List<Book>> SearchBooks(string searchTerm)
        {
            return _libraryService.FindBooks(searchTerm);
        }

        [HttpGet("{id}")]
        public ActionResult<Book> Get(int id)
        {
            Book book = _libraryService.GetBookById(id);
            if (book == null)
            {
                return NotFound($"Book with ID {id} was not found");
            }

            return book;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Author author = _libraryService.GetAuthorById((int)book.AuthorId);
            if (author == null)
            {
                return NotFound($"Author with ID {(int)book.AuthorId} was not found");
            }

            _libraryService.AddBook(book);

            return Ok($"Book {book.Title} of {author.FirstName} {author.LastName} has been added to the database.");
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Author author = _libraryService.GetAuthorById((int)book.AuthorId);
            if (author == null)
            {
                return NotFound($"Author with ID {(int)book.AuthorId} was not found");
            }

            Book oldBook = _libraryService.UpdateBook(id, book);
            if (oldBook == null)
            {
                return NotFound($"Book with ID {id} was not found");
            }

            return Ok($"Book {oldBook.Title}({oldBook.Pages}) of category {oldBook.Category} and publication date of {oldBook.Date} from {oldBook.Author.FirstName} {oldBook.Author.LastName} has been changed to {book.Title}({book.Pages}) of category {book.Category} and publication date of {book.Date} from {author.FirstName} {author.LastName}");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Book book = _libraryService.DeleteBook(id);
            if (book == null)
            {
                return NotFound($"Book with ID {id} was not found");
            }

            return Ok($"Book {book.Title} has been deleted.");
        }
    }
}
