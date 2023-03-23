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
        List<Book> FindBooks(string searchTerm);
        void InitializeAuthors(int numberOfAuthors);
        void InitializeBooks(int numberOfBooks, List<Author> authors);
    }

    public class LibraryService : ILibraryService
    {
        private LibraryDatalayer _libraryDatalayer;

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
            _libraryDatalayer = new LibraryDatalayer();
        }

        public List<Author> GetAllAuthors()
        {
            _libraryDatalayer.OpenConnection();

            List<Author> authors = _libraryDatalayer.GetAllAuthors();

            _libraryDatalayer.CloseConnection();

            return authors;
        }

        public List<Book> GetAllBooks()
        {
            _libraryDatalayer.OpenConnection();

            List<Book> books = _libraryDatalayer.GetAllBooks();

            _libraryDatalayer.CloseConnection();

            return books;
        }

        public Author GetAuthorById(int id)
        {
            _libraryDatalayer.OpenConnection();

            Author author = _libraryDatalayer.GetAuthorById(id);

            _libraryDatalayer.CloseConnection();

            return author;
        }

        public Book GetBookById(int id)
        {
            _libraryDatalayer.OpenConnection();

            Book book = _libraryDatalayer.GetBookById(id);

            _libraryDatalayer.CloseConnection();

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

            _libraryDatalayer.AddAuthor(author);

            _libraryDatalayer.CloseConnection();
        }

        public void AddBook(Book book)
        {
            _libraryDatalayer.OpenConnection();

            _libraryDatalayer.AddBook(book);

            _libraryDatalayer.CloseConnection();
        }

        public Author UpdateAuthor(int id, Author updatedAuthor)
        {
            _libraryDatalayer.OpenConnection();

            Author existingAuthor = _libraryDatalayer.GetAuthorById(id);

            if (existingAuthor != null)
            {
                _libraryDatalayer.UpdateAuthor(existingAuthor, updatedAuthor);
            }

            _libraryDatalayer.CloseConnection();

            return existingAuthor;
        }

        public Book UpdateBook(int id, Book updatedBook)
        {
            _libraryDatalayer.OpenConnection();

            Book existingBook = _libraryDatalayer.GetBookById(id);

            if (existingBook != null)
            {
                _libraryDatalayer.UpdateBook(existingBook, updatedBook);
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
            }

            _libraryDatalayer.CloseConnection();

            return book;
        }

        public List<Book> FindBooks(string searchTerm)
        {
            _libraryDatalayer.OpenConnection();

            List<Book> books = _libraryDatalayer.FindBooks(searchTerm);

            _libraryDatalayer.CloseConnection();

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

                _libraryDatalayer.AddAuthor(author);
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

                _libraryDatalayer.AddBook(book);
            }

            _libraryDatalayer.CloseConnection();
        }
    }
}
