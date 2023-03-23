using WebAPI.Model;
using WebAPI.Helpers;

namespace WebAPI.Services
{
    public interface IWorkerService
    {
        Job AddBooksInQueue(List<Book> books);
        Job FindJobById(Guid id);
    }

    public class WorkerService : IWorkerService
    {
        ParsingQueue _queue = new ParsingQueue();

        public Job AddBooksInQueue(List<Book> books)
        {
            Job job = new()
            {
                Id = Guid.NewGuid(),
                Status = "Queued",
                Books = books
            };

            _queue.AddJobInQueue(job);

            return job;
        }

        public Job FindJobById(Guid id)
        {
            return _queue.FindJobById(id);
        }
    }
}