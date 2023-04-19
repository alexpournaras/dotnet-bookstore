using Microsoft.AspNetCore.Mvc;
using WebAPI.Model;
using WebAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParseController : ControllerBase
    {
        private IWorkerService _workerService;

        public ParseController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        [HttpPost]
        [Authorize(Roles = "Developer")]
        public ActionResult Post([FromBody] List<Book> books)
        {
            Job job = _workerService.AddBooksInQueue(books);

            return Ok($"A job has been queued with ID: {job.Id}");
        }
    }
}