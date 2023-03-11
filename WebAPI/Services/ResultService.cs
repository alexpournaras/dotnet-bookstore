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
    }

    public class ResultService : IResultService
    {
        private AuthorsDatalayer _authorsDatalayer;

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
    }
}
