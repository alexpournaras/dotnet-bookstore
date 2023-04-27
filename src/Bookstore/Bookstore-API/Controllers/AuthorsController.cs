using Microsoft.AspNetCore.Mvc;
using BookstoreAPI.Models;
using BookstoreAPI.Filters;
using BookstoreAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace BookstoreAPI.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   public class AuthorsController : ControllerBase
   {
       private IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public ActionResult<List<Author>> GetAllAuthors()
        {
            try
            {
                var authors = _authorService.GetAll();
                return Ok(authors);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Author> GetAuthor(int id)
        {
            try
            {
                var author = _authorService.Get(id);
                if (author == null)
                {
                    return NotFound(new { message = "Author not found!" });
                }

                return Ok(author);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public ActionResult<Author> CreateAuthor(Author author)
        {
            try
            {
                var res = _authorService.Add(author);
                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch]
        public ActionResult<Author> UpdateAuthor(UpdateAuthorEntity author)
        {
            try
            {
                var res = _authorService.Update(author);
                if (res == 0)
                {
                    return NotFound(new { message = "Author not found!" });
                }

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
                var res = _authorService.Delete(id);
                if (res == 0)
                {
                    return NotFound(new { message = "Author not found!" });
                }

                return Ok("{ rows_affected: " + res + " }");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

   }
}
