using WebAPI.Datalayer;
using WebAPI.Caching;
using WebAPI.Model;
using System.Text.Json;

namespace WebAPI.Services
{
    public interface ILibraryService
    {
        List<Author> GetAllAuthors();
        List<Book> GetAllBooks();
        Author GetAuthorById(int id);
        Book GetBookById(int id);
        List<Book> GetBooksByAuthor(int authorId);
        void AddAuthor(Author author);
        void AddBook(Book book);
        Author UpdateAuthor(int id, Author author);
        Book UpdateBook(int id, Book book);
        Author DeleteAuthor(int id);
        Book DeleteBook(int id);
        List<Book> FindBooks(string searchTerm);
        void InitializeAuthors(int numberOfAuthors);
        void InitializeBooks(int numberOfBooks, List<Author> authors);
    }

    public class LibraryService : ILibraryService
    {
        private static readonly string[] FirstNames = new[]
        {
            "Jim", "John", "Alex", "Teo", "George", "Takis"
        };

        private static readonly string[] LastNames = new[]
        {
            "Papadopoulos", "Papas", "Ioannidis", "Konstantinidis", "Aggelou", "Gonias"
        };

        private static readonly string[] Countries = new[]
        {
            "Greece", "Italy", "Germany", "Cyprus", "France", "Switzerland"
        };

        private static readonly string[] Titles = new[]
        {
            "Harry Potter", "Lord of the Rings", "The Lion", "Pinocchio", "Alice in Wonderland"
        };

        private static readonly string[] Categories = new[]
        {
            "Adventure", "Crime", "Fantasy", "Historical", "Horror", "Humour"
        };

        private LibraryDatalayer _libraryDatalayer;
        private RedisCacheManager _redisCacheManager;

        public LibraryService(LibraryDatalayer libraryDatalayer, RedisCacheManager cacheManager)
        {
            _libraryDatalayer = libraryDatalayer;
            _redisCacheManager = cacheManager;
        }

        public List<Author> GetAllAuthors()
        {
            List<Author> authors = new List<Author>();
            var redisKeys = _redisCacheManager.Connection.GetServer(_redisCacheManager.Connection.GetEndPoints().First()).Keys(pattern: "author:*");

            foreach (var redisKey in redisKeys)
            {
                string serializedAuthor = _redisCacheManager.Get<string>(redisKey);

                if (!string.IsNullOrEmpty(serializedAuthor))
                {
                    Author author = JsonSerializer.Deserialize<Author>(serializedAuthor);
                    authors.Add(author);
                }
            }

            if (authors.Count == 0)
            {
                _libraryDatalayer.OpenConnection();
                authors = _libraryDatalayer.GetAllAuthors();
                _libraryDatalayer.CloseConnection();

                foreach (Author author in authors)
                {
                    _redisCacheManager.Set($"author:{author.Id}", author);
                }
            }

            return authors;
        }

        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();
            var redisKeys = _redisCacheManager.Connection.GetServer(_redisCacheManager.Connection.GetEndPoints().First()).Keys(pattern: "book:*");

            foreach (var redisKey in redisKeys)
            {
                string serializedBook = _redisCacheManager.Get<string>(redisKey);

                if (!string.IsNullOrEmpty(serializedBook))
                {
                    Book book = JsonSerializer.Deserialize<Book>(serializedBook);
                    books.Add(book);
                }
            }

            if (books.Count == 0)
            {
                _libraryDatalayer.OpenConnection();
                books = _libraryDatalayer.GetAllBooks();
                _libraryDatalayer.CloseConnection();

                foreach (Book book in books)
                {
                    _redisCacheManager.Set($"book:{book.Id}", book);
                }
            }

            return books;
        }

        public Author GetAuthorById(int id)
        {
            Author author = null;
            string serializedAuthor = _redisCacheManager.Get<string>($"author:{id}");

            if (!string.IsNullOrEmpty(serializedAuthor))
            {
                author = JsonSerializer.Deserialize<Author>(serializedAuthor);
            }
            else
            {
                _libraryDatalayer.OpenConnection();
                author = _libraryDatalayer.GetAuthorById(id);
                _libraryDatalayer.CloseConnection();

                if (author != null)
                {
                    _redisCacheManager.Set($"author:{id}", author);
                }
            }

            return author;
        }

        public Book GetBookById(int id)
        {
            Book book = null;
            string serializedBook = _redisCacheManager.Get<string>($"book:{id}");

            if (!string.IsNullOrEmpty(serializedBook))
            {
                book = JsonSerializer.Deserialize<Book>(serializedBook);
            }
            else
            {
                _libraryDatalayer.OpenConnection();
                book = _libraryDatalayer.GetBookById(id);
                _libraryDatalayer.CloseConnection();

                if (book != null)
                {
                    _redisCacheManager.Set($"book:{id}", book);
                }
            }

            return book;
        }

        public List<Book> GetBooksByAuthor(int authorId)
        {
            _libraryDatalayer.OpenConnection();

            List<Book> books = _libraryDatalayer.GetBooksByAuthor(authorId);

            _libraryDatalayer.CloseConnection();

            return books;
        }

        public void AddAuthor(Author author)
        {
            _libraryDatalayer.OpenConnection();

            int authorId = _libraryDatalayer.AddAuthor(author);
            author.Id = authorId;

            _libraryDatalayer.CloseConnection();

            _redisCacheManager.Set($"author:{author.Id}", author);
        }

        public void AddBook(Book book)
        {
            _libraryDatalayer.OpenConnection();

            int bookId = _libraryDatalayer.AddBook(book);
            book.Id = bookId;

            _libraryDatalayer.CloseConnection();

            _redisCacheManager.Set($"book:{book.Id}", book);
        }

        public Author UpdateAuthor(int id, Author updatedAuthor)
        {
            updatedAuthor.Id = id;
            _libraryDatalayer.OpenConnection();

            Author existingAuthor = _libraryDatalayer.GetAuthorById(id);

            if (existingAuthor != null)
            {
                _libraryDatalayer.UpdateAuthor(updatedAuthor);
                _redisCacheManager.Set($"author:{id}", updatedAuthor);
            }

            _libraryDatalayer.CloseConnection();

            return existingAuthor;
        }

        public Book UpdateBook(int id, Book updatedBook)
        {
            updatedBook.Id = id;
            _libraryDatalayer.OpenConnection();

            Book existingBook = _libraryDatalayer.GetBookById(id);

            if (existingBook != null)
            {
                _libraryDatalayer.UpdateBook(updatedBook);
                _redisCacheManager.Set($"book:{id}", updatedBook);
            }

            _libraryDatalayer.CloseConnection();

            return existingBook;
        }

        public Author DeleteAuthor(int id)
        {
            _libraryDatalayer.OpenConnection();

            Author author = _libraryDatalayer.GetAuthorById(id);

            if (author != null)
            {
                _libraryDatalayer.DeleteAuthor(author);
                _redisCacheManager.Remove($"author:{id}");
            }

            _libraryDatalayer.CloseConnection();

            return author;
        }

        public Book DeleteBook(int id)
        {
            _libraryDatalayer.OpenConnection();

            Book book = _libraryDatalayer.GetBookById(id);

            if (book != null)
            {
                _libraryDatalayer.DeleteBook(book);
                _redisCacheManager.Remove($"book:{id}");
            }

            _libraryDatalayer.CloseConnection();

            return book;
        }

        public List<Book> FindBooks(string searchTerm)
        {
            List<Book> books = _redisCacheManager.Get<List<Book>>($"search:{searchTerm}");

            if (books == null)
            {
                _libraryDatalayer.OpenConnection();
                books = _libraryDatalayer.FindBooks(searchTerm);
                _libraryDatalayer.CloseConnection();

                _redisCacheManager.Set($"search:{searchTerm}", books, TimeSpan.FromMinutes(30));
            }

            return books;
        }

        public void InitializeAuthors(int numberOfAuthors)
        {
            _libraryDatalayer.OpenConnection();

            Random randomizer = new Random();

            for (int i = 1; i <= numberOfAuthors; i++)
            {
                Author author = new Author();

                author.FirstName = FirstNames[randomizer.Next(FirstNames.Length)];
                author.LastName = LastNames[randomizer.Next(LastNames.Length)];
                author.Country = Countries[randomizer.Next(Countries.Length)];

                int authorId = _libraryDatalayer.AddAuthor(author);
                author.Id = authorId;

                _redisCacheManager.Set($"author:{authorId}", author);
            }

            _libraryDatalayer.CloseConnection();
        }

        public void InitializeBooks(int numberOfBooks, List<Author> authors)
        {
            _libraryDatalayer.OpenConnection();

            Random randomizer = new Random();

            for (int i = 1; i <= numberOfBooks; i++)
            {
                Book book = new Book();

                DateTime startDate = DateTime.Now.AddDays(-365);
                book.Date = startDate.AddDays(randomizer.Next(365)).ToString("yyyy-MM-dd");

                Author author = authors[randomizer.Next(authors.Count)];
                book.AuthorId = author.Id;

                book.Title = Titles[randomizer.Next(Titles.Length)];
                book.Category = Categories[randomizer.Next(Categories.Length)];
                book.Pages = randomizer.Next(999);

                int bookId = _libraryDatalayer.AddBook(book);
                book.Id = bookId;

                _redisCacheManager.Set($"book:{bookId}", book);
            }

            _libraryDatalayer.CloseConnection();
        }
    }
}
