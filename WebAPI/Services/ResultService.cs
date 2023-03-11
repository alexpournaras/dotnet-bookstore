using WebAPI.Datalayer;
using WebAPI.Model;

namespace WebAPI.Services
{
    
    public interface IResultService
    {
        List<Author> GetAllAuthors();
        Author GetAuthorById(int id);
        void AddAuthor(Author author);
        Author UpdateAuthor(int id, Author author);
        Author DeleteAuthor(int id);
        void InitializeAuthors(int numberOfAuthors);
    }

    public class ResultService : IResultService
    {
        private AuthorsDatalayer _authorsDatalayer;

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
        }

        public List<Author> GetAllAuthors()
        {
            _authorsDatalayer.OpenConnection();

            List<Author> authors = _authorsDatalayer.GetAllAuthors();

            _authorsDatalayer.CloseConnection();

            return authors;
        }

        public Author GetAuthorById(int id)
        {
            _authorsDatalayer.OpenConnection();

            Author author = _authorsDatalayer.GetAuthorById(id);

            _authorsDatalayer.CloseConnection();

            return author;
        }

        public void AddAuthor(Author author)
        {
            _authorsDatalayer.OpenConnection();

            _authorsDatalayer.AddAuthor(author);

            _authorsDatalayer.CloseConnection();
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
