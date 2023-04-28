using BookstoreAPI.Models;
using BookstoreAPI.Helpers;
using BookstoreAPI.Repositories;

namespace BookstoreAPI.Services
{
   public interface IWorkerService
   {
       Job AddBooksInQueue(List<UpdateBookEntity> books);
       Job FindJobById(Guid id);
   }

   public class WorkerService : IWorkerService
   {
        private ParsingQueue _queue;
        private DatabaseHelper _databaseHelper;

        public WorkerService(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
            _queue = new ParsingQueue(_databaseHelper);
        }
        public Job AddBooksInQueue(List<UpdateBookEntity> books)
        {
            // Create a new job
            Job job = new()
            {
                Id = Guid.NewGuid(),
                Status = "Queued",
                Books = books
            };

            // Add job in queue
            _queue.AddJobInQueue(job);

            return job;
        }

       public Job FindJobById(Guid id)
       {
            return _queue.FindJobById(id);
            // return null;
       }
   }
}