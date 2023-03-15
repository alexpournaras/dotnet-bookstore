using WebAPI.Datalayer;
using WebAPI.Model;

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
        void InitializeAuthors(int numberOfAuthors);
        void InitializeBooks(int numberOfBooks, List<Author> authors);
    }

    public class LibraryService : ILibraryService
    {
        private AuthorsDatalayer _authorsDatalayer;
        private BooksDatalayer _booksDatalayer;

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

        public LibraryService()
        {
            _authorsDatalayer = new AuthorsDatalayer();
            _booksDatalayer = new BooksDatalayer();
        }

        public List<Author> GetAllAuthors()
        {
            _authorsDatalayer.OpenConnection();

            List<Author> authors = _authorsDatalayer.GetAllAuthors();

            _authorsDatalayer.CloseConnection();

            return authors;
        }

        public List<Book> GetAllBooks()
        {
            _booksDatalayer.OpenConnection();

            List<Book> books = _booksDatalayer.GetAllBooks();

            _booksDatalayer.CloseConnection();

            return books;
        }

        public Author GetAuthorById(int id)
        {
            _authorsDatalayer.OpenConnection();

            Author author = _authorsDatalayer.GetAuthorById(id);

            _authorsDatalayer.CloseConnection();

            return author;
        }

        public Book GetBookById(int id)
        {
            _booksDatalayer.OpenConnection();

            Book book = _booksDatalayer.GetBookById(id);

            _booksDatalayer.CloseConnection();

            return book;
        }

        public List<Book> GetBooksByAuthor(int authorId)
        {
            _booksDatalayer.OpenConnection();

            List<Book> books = _booksDatalayer.GetBooksByAuthor(authorId);

            _booksDatalayer.CloseConnection();

            return books;
        }

        public void AddAuthor(Author author)
        {
            _authorsDatalayer.OpenConnection();

            _authorsDatalayer.AddAuthor(author);

            _authorsDatalayer.CloseConnection();
        }

        public void AddBook(Book book)
        {
            _booksDatalayer.OpenConnection();

            _booksDatalayer.AddBook(book);

            _booksDatalayer.CloseConnection();
        }

        public Author UpdateAuthor(int id, Author updatedAuthor)
        {
            _authorsDatalayer.OpenConnection();

            Author existingAuthor = _authorsDatalayer.GetAuthorById(id);

            if (existingAuthor != null)
            {
                _authorsDatalayer.UpdateAuthor(existingAuthor, updatedAuthor);
            }

            _authorsDatalayer.CloseConnection();

            return existingAuthor;
        }

        public Book UpdateBook(int id, Book updatedBook)
        {
            _booksDatalayer.OpenConnection();

            Book existingBook = _booksDatalayer.GetBookById(id);

            if (existingBook != null)
            {
                _booksDatalayer.UpdateBook(existingBook, updatedBook);
            }

            _booksDatalayer.CloseConnection();

            return existingBook;
        }

        public Author DeleteAuthor(int id)
        {
            _authorsDatalayer.OpenConnection();

            Author author = _authorsDatalayer.GetAuthorById(id);

            if (author != null)
            {
                _authorsDatalayer.DeleteAuthor(author);
            }

            _authorsDatalayer.CloseConnection();

            return author;
        }

        public Book DeleteBook(int id)
        {
            _booksDatalayer.OpenConnection();

            Book book = _booksDatalayer.GetBookById(id);

            if (book != null)
            {
                _booksDatalayer.DeleteBook(book);
            }

            _booksDatalayer.CloseConnection();

            return book;
        }

        public void InitializeAuthors(int numberOfAuthors)
        {
            _authorsDatalayer.OpenConnection();

            Random randomizer = new Random();

            for (int i = 1; i <= numberOfAuthors; i++)
            {
                Author author = new Author();

                author.FirstName = FirstNames[randomizer.Next(FirstNames.Length)];
                author.LastName = LastNames[randomizer.Next(LastNames.Length)];
                author.Country = Countries[randomizer.Next(Countries.Length)];

                _authorsDatalayer.AddAuthor(author);
            }

            _authorsDatalayer.CloseConnection();
        }

        public void InitializeBooks(int numberOfBooks, List<Author> authors)
        {
            _booksDatalayer.OpenConnection();

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

                _booksDatalayer.AddBook(book);
            }

            _booksDatalayer.CloseConnection();
        }
    }
}
