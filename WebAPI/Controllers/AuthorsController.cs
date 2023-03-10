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
    }
}
