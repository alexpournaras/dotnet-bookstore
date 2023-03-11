using Microsoft.AspNetCore.Mvc;
using WebAPI.Model;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorsController : ControllerBase
    {
        private IResultService _resultService;

        public AuthorsController(IResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpGet]
        public ActionResult<List<Author>> GetAllAuthors()
        {
            return _resultService.GetAllAuthors();
        }

        [HttpGet("{id}")]
        public ActionResult<Author> Get(int id)
        {
            Author author = _resultService.GetAuthorById(id);

            if (author == null)
            {
                return NotFound($"Author with ID {id} was not found");
            }

            return author;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _resultService.AddAuthor(author);

            return Ok($"Author {author.FirstName} {author.LastName} has been added to the database.");
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Author oldAuthor = _resultService.UpdateAuthor(id, author);

            if (oldAuthor == null)
            {
                return NotFound($"Author with ID {id} was not found");
            }

            return Ok($"Author {oldAuthor.FirstName} {oldAuthor.LastName} from {oldAuthor.Country} with {oldAuthor.Books} books, has been updated to {author.FirstName} {author.LastName} from {author.Country} with {author.Books} books.");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Author author = _resultService.DeleteAuthor(id);

            if (author == null)
            {
                return NotFound($"Author with ID {id} was not found");
            }

            return Ok($"Author {author.FirstName} {author.LastName} has been deleted.");
        }
    }
}
