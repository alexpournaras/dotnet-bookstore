using WebAPI.Datalayer;
using WebAPI.Model;

namespace WebAPI.Services
{
    
    public interface IResultService
    {
        List<Author> GetAllAuthors();
        List<Book> GetAllBooks();
        Author GetAuthorById(int id);
        Book GetBookById(int id);
        void AddAuthor(Author author);
        void AddBook(Book book);
        Author UpdateAuthor(int id, Author author);
        Book UpdateBook(int id, Book book);
        Author DeleteAuthor(int id);
        Book DeleteBook(int id);
        void InitializeAuthors(int numberOfAuthors);
    }

    public class ResultService : IResultService
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

        public ResultService()
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

            for (int i = 1; i <= numberOfAuthors; i++)
            {
                Author author = new Author();

                author.FirstName = FirstNames[new Random().Next(FirstNames.Length)];
                author.LastName = LastNames[new Random().Next(LastNames.Length)];
                author.Country = Countries[new Random().Next(Countries.Length)];

                _authorsDatalayer.AddAuthor(author);
            }

            _authorsDatalayer.CloseConnection();
        }
    }
}
