//using BookstoreAPI.Models;
//using BookstoreAPI.Helpers;

//namespace BookstoreAPI.Services
//{
//    public interface IWorkerService
//    {
//        Job AddBooksInQueue(List<Book> books);
//        Job FindJobById(Guid id);
//    }

//    public class WorkerService : IWorkerService
//    {
//        // The queue that contains the jobs
//        ParsingQueue _queue = new ParsingQueue();

//        public Job AddBooksInQueue(List<Book> books)
//        {
//            // Create a new job
//            Job job = new()
//            {
//                Id = Guid.NewGuid(),
//                Status = "Queued",
//                Books = books
//            };

//            // Add job in queue
//            _queue.AddJobInQueue(job);

//            return job;
//        }

//        public Job FindJobById(Guid id)
//        {
//            // Find job to show status
//            return _queue.FindJobById(id);
//        }
//    }
//}