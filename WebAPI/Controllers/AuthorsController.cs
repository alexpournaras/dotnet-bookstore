using Microsoft.AspNetCore.Mvc;
using WebAPI.Model;
using WebAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorsController : ControllerBase
    {
        private ILibraryService _libraryService;

        public AuthorsController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpGet]
        [Authorize(Roles = "Developer,User")]
        public ActionResult<List<Author>> GetAllAuthors()
        {
            return _libraryService.GetAllAuthors();
        }

        [HttpGet("init/{numberOfAuthors}")]
        [Authorize(Roles = "Developer")]
        public ActionResult<List<Author>> InitializeAuthors(int numberOfAuthors)
        {
            _libraryService.InitializeAuthors(numberOfAuthors);
            
            return _libraryService.GetAllAuthors();
        }

        [HttpGet("{id}")]
        public ActionResult<Author> Get(int id)
        {
            Author author = _libraryService.GetAuthorById(id);
            if (author == null)
            {
                return NotFound($"Author with ID {id} was not found");
            }

            return author;
        }

        [HttpPost]
        [Authorize(Roles = "Developer")]
        public ActionResult Post([FromBody] Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _libraryService.AddAuthor(author);

            return Ok($"Author {author.FirstName} {author.LastName} has been added to the database.");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Developer")]
        public ActionResult Put(int id, [FromBody] Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Author oldAuthor = _libraryService.UpdateAuthor(id, author);
            if (oldAuthor == null)
            {
                return NotFound($"Author with ID {id} was not found");
            }

            return Ok($"Author {oldAuthor.FirstName} {oldAuthor.LastName} from {oldAuthor.Country}, has been updated to {author.FirstName} {author.LastName} from {author.Country}.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Developer")]
        public ActionResult Delete(int id)
        {
            List<Book> books = _libraryService.GetBooksByAuthor(id);
            if (books.Count > 0)
            {
                return BadRequest($"You have to delete all books from this author first!");
            }

            Author author = _libraryService.DeleteAuthor(id);
            if (author == null)
            {
                return NotFound($"Author with ID {id} was not found");
            }

            return Ok($"Author {author.FirstName} {author.LastName} has been deleted.");
        }
    }
}
