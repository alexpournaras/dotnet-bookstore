using WebAPI.Datalayer;
using WebAPI.Model;

namespace WebAPI.Services
{

    public interface IResultService
    {
        List<Author> GetAllAuthors();
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
    }
}
