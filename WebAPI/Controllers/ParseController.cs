using Microsoft.AspNetCore.Mvc;
using WebAPI.Model;
using WebAPI.Services;

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
        public ActionResult Post([FromBody] List<Book> books)
        {
            Job job = _workerService.AddBooksInQueue(books);

            return Ok($"A job has been queued with ID: {job.Id}");
        }
    }
}