using Microsoft.AspNetCore.Mvc;
using BookstoreAPI.Models;
using BookstoreAPI.Filters;
using BookstoreAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace BookstoreAPI.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   public class ParseController : ControllerBase
   {
       private IWorkerService _workerService;

       public ParseController(IWorkerService workerService)
       {
           _workerService = workerService;
       }

       [HttpPost("books")]
    //    [IPLocationLookup]
    //    [Authorize(Roles = "Developer")]
       public ActionResult Parse(List<UpdateBookEntity> books)
       {
            try
            {
                var job = _workerService.AddBooksInQueue(books);
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