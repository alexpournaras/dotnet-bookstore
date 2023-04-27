using BookstoreAPI.Models;
using BookstoreAPI.Repositories;

namespace BookstoreAPI.Services
{
    public interface IAuthorService
    {
        public List<Author> GetAll();
        public Author Get(int id);
        public Author Add(Author author);
        public int Update(UpdateAuthorEntity author);
        public int Delete(int id);
    }

    public class AuthorService : IAuthorService
    {
        private IAuthorRepository _authorRepository;
        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public List<Author> GetAll()
        {
            return _authorRepository.GetAllAuthors();
        }

        public Author Get(int id)
        {
            return _authorRepository.GetAuthor(id);
        }

        public Author Add(Author author)
        {
            return _authorRepository.InsertAuthor(author);
        }

        public int Update(UpdateAuthorEntity author)
        {
            return _authorRepository.UpdateAuthor(author);
        }
        public int Delete(int id)
        {
            return _authorRepository.DeleteAuthor(id);
        }
    }
}
