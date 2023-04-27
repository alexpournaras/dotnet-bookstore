//using Microsoft.AspNetCore.Mvc;
//using BookstoreAPI.Models;
//using BookstoreAPI.Services;

//namespace BookstoreAPI.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class JobsController : ControllerBase
//    {
//        private IWorkerService _workerService;

//        public JobsController(IWorkerService workerService)
//        {
//            _workerService = workerService;
//        }

//        [HttpGet("{id}")]
//        public ActionResult<Job> Get(Guid id)
//        {
//            Job job = _workerService.FindJobById(id);
//            if (job == null)
//            {
//                return NotFound($"Job with ID {id} was not found");
//            }

//            return Ok($"The status of job with ID {id} is {job.Status}.");
//        }
//    }
////}