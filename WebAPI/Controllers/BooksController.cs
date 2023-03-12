using Microsoft.AspNetCore.Mvc;
using WebAPI.Model;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private IResultService _resultService;

        public BooksController(IResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpGet]
        public ActionResult<List<Book>> GetAllBooks()
        {
            return _resultService.GetAllBooks();
        }

        [HttpGet("init/{numberOfBooks}")]
        public ActionResult<List<Book>> InitializeBooks(int numberOfBooks)
        {
            List<Author> authors = _resultService.GetAllAuthors();

            if (authors.Count == 0)
            {
                return NotFound($"You cannot initialize books without authors. First add some authors!");
            }

            _resultService.InitializeBooks(numberOfBooks, authors);

            return _resultService.GetAllBooks();
        }

        [HttpGet("{id}")]
        public ActionResult<Book> Get(int id)
        {
            Book book = _resultService.GetBookById(id);

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

            Author author = _resultService.GetAuthorById((int)book.AuthorId);
            if (author == null)
            {
                return NotFound($"Author with ID {(int)book.AuthorId} was not found");
            }

            _resultService.AddBook(book);

            return Ok($"Book {book.Title} of {author.FirstName} {author.LastName} has been added to the database.");
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Author author = _resultService.GetAuthorById((int)book.AuthorId);
            if (author == null)
            {
                return NotFound($"Author with ID {(int)book.AuthorId} was not found");
            }

            Book oldBook = _resultService.UpdateBook(id, book);
            if (oldBook == null)
            {
                return NotFound($"Book with ID {id} was not found");
            }

            return Ok($"Book {oldBook.Title}({oldBook.Pages}) of category {oldBook.Category} and publication date of {oldBook.Date} from {oldBook.Author.FirstName} {oldBook.Author.LastName} has been changed to {book.Title}({book.Pages}) of category {book.Category} and publication date of {book.Date} from {author.FirstName} {author.LastName}");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Book book = _resultService.DeleteBook(id);

            if (book == null)
            {
                return NotFound($"Book with ID {id} was not found");
            }

            return Ok($"Book {book.Title} has been deleted.");
        }
    }
}
