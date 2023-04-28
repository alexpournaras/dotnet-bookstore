using BookstoreAPI.Models;
using BookstoreAPI.Repositories;
using BookstoreAPI.Helpers;
using System.Text.Json;

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
        private RedisCacheManager _redisCacheManager;

        public AuthorService(IAuthorRepository authorRepository, RedisCacheManager cacheManager)
        {
            _authorRepository = authorRepository;
            _redisCacheManager = cacheManager;
        }

        public List<Author> GetAll()
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
                authors = _authorRepository.GetAllAuthors();
                foreach (Author author in authors)
                {
                    _redisCacheManager.Set($"author:{author.Id}", JsonSerializer.Serialize(author));
                }
            }

            return authors;
        }

        public Author Get(int id)
        {
            Author author = null;
            string serializedAuthor = _redisCacheManager.Get<string>($"author:{id}");

            if (!string.IsNullOrEmpty(serializedAuthor))
            {
                author = JsonSerializer.Deserialize<Author>(serializedAuthor);
            }
            else
            {
                author = _authorRepository.GetAuthor(id);
                if (author != null)
                {
                    _redisCacheManager.Set($"author:{id}", JsonSerializer.Serialize(author));
                }
            }

            return author;
        }

        public Author Add(Author author)
        {
            Author newAuthor = _authorRepository.InsertAuthor(author);
            if (newAuthor != null)
            {
                _redisCacheManager.Set($"author:{newAuthor.Id}", JsonSerializer.Serialize(newAuthor));
            }

            return newAuthor;
        }

        public int Update(UpdateAuthorEntity author)
        {
            var res = _authorRepository.UpdateAuthor(author);
            if (res > 0)
            {
                string serializedAuthor = _redisCacheManager.Get<string>($"author:{author.Id}");
                if (!string.IsNullOrEmpty(serializedAuthor))
                {
                    Author cachedAuthor = JsonSerializer.Deserialize<Author>(serializedAuthor);

                    foreach (var property in author.GetType().GetProperties())
                    {
                        var value = property.GetValue(author);
                        if (value != null)
                        {
                            cachedAuthor.GetType().GetProperty(property.Name).SetValue(cachedAuthor, value);
                        }
                    }

                    _redisCacheManager.Set($"author:{cachedAuthor.Id}", JsonSerializer.Serialize(cachedAuthor));
                }
            }

            return res;
        }

        public int Delete(int id)
        {
            var res = _authorRepository.DeleteAuthor(id);
            if (res > 0)
            {
                _redisCacheManager.Remove($"author:{id}");
            }

            return res;
        }
    }
}
