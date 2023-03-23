using System.Collections.Concurrent;
using WebAPI.Datalayer;
using WebAPI.Model;

namespace WebAPI.Helpers
{
    public class ParsingQueue
    {
        int _queueSize = 10;
        int _numOfWorkers = 2;
        private LibraryDatalayer _libraryDatalayer = new LibraryDatalayer();
        private ConcurrentDictionary<Guid, Job> _jobDictionary = new ConcurrentDictionary<Guid, Job>();

        public void AddJobInQueue(Job job)
        {
            _jobDictionary.TryAdd(job.Id, job);

            Task.Run(() => ParseJobBooks(job));
        }

        public Job FindJobById(Guid id)
        {
            _jobDictionary.TryGetValue(id, out Job job);

            return job;
        }

        public void ParseJobBooks(Job job)
        {
            job.Status = "In-progress";

            var batches = new List<List<Book>>();

            for (int i = 0; i < job.Books.Count; i += _queueSize)
            {
                batches.Add(job.Books.GetRange(i, Math.Min(_queueSize, job.Books.Count - i)));
            }

            _libraryDatalayer.OpenConnection();

            Parallel.ForEach(batches, new ParallelOptions { MaxDegreeOfParallelism = _numOfWorkers }, (batch) =>
            {
                foreach (Book book in batch)
                {
                    try
                    {
                        _libraryDatalayer.InsertOrUpdateBook(book);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        job.Status = "Failed";
                    }
                }
            });

            _libraryDatalayer.CloseConnection();

            if (job.Status != "Failed") job.Status = "Completed";
        }
    }
}