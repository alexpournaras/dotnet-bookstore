using Microsoft.AspNetCore.Mvc;
using BookstoreAPI.Models;
using BookstoreAPI.Services;

namespace BookstoreAPI.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   public class JobsController : ControllerBase
   {
       private IWorkerService _workerService;

       public JobsController(IWorkerService workerService)
       {
           _workerService = workerService;
       }

       [HttpGet("{id}")]
       public ActionResult<Job> Get(Guid id)
       {
            try
            {
                var job = _workerService.FindJobById(id);
                if (job == null)
                {
                    return NotFound(new { message = "Job not found!" });
                }

                return Ok(new { id = job.Id, status = job.Status });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
       }
   }
}