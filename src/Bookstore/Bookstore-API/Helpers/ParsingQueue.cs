//using System.Collections.Concurrent;
//using BookstoreAPI.Datalayer;
//using BookstoreAPI.Models;

//namespace BookstoreAPI.Helpers
//{
//    public class ParsingQueue
//    {
//        int _queueSize = 10;
//        int _numOfWorkers = 2;
//        private LibraryDatalayer _libraryDatalayer = new LibraryDatalayer();
//        private ConcurrentDictionary<Guid, Job> _jobDictionary = new ConcurrentDictionary<Guid, Job>();

//        public void AddJobInQueue(Job job)
//        {
//            // Add job in dictionary of queue
//            _jobDictionary.TryAdd(job.Id, job);

//            // Parse books when a worker becomes available
//            Task.Run(() => ParseJobBooks(job));
//        }

//        public Job FindJobById(Guid id)
//        {
//            // Find the job in dictionary
//            _jobDictionary.TryGetValue(id, out Job job);

//            return job;
//        }

//        public void ParseJobBooks(Job job)
//        {
//            // Begin process of parsing. Set the in-progress status.
//            job.Status = "In-progress";

//            // Create batches with books based on the _queueSize
//            var batches = new List<List<Book>>();

//            for (int i = 0; i < job.Books.Count; i += _queueSize)
//            {
//                batches.Add(job.Books.GetRange(i, Math.Min(_queueSize, job.Books.Count - i)));
//            }

//            _libraryDatalayer.OpenConnection();

//            // Run the bulk insertion or update in parallel with configurable number of workers
//            Parallel.ForEach(batches, new ParallelOptions { MaxDegreeOfParallelism = _numOfWorkers }, (batch) =>
//            {
//                foreach (Book book in batch)
//                {
//                    try
//                    {
//                        _libraryDatalayer.InsertOrUpdateBook(book);
//                    }
//                    catch (Exception ex)
//                    {
//                        // Set the job as failed even if only one book action failed.
//                        job.Status = "Failed";

//                        Console.WriteLine(ex);
//                    }
//                }
//            });

//            _libraryDatalayer.CloseConnection();

//            // Set status as completed if job succeed
//            if (job.Status != "Failed") job.Status = "Completed";
//        }
//    }
//}