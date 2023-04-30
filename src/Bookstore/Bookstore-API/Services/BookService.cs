using BookstoreAPI.Models;
using BookstoreAPI.Repositories;
using BookstoreAPI.Helpers;
using System.Text.Json;

namespace BookstoreAPI.Services
{
    public interface IBookService
    {
        public List<Book> GetAll();
        public Book Get(int id);
        public Book Add(Book book);
        public int Update(UpdateBookEntity book);
        public int Delete(int id);
        public List<Book> SearchBooks(string searchTerm);
    }

    public class BookService : IBookService
    {
        private IBookRepository _bookRepository;
        private RedisCacheManager _redisCacheManager;

        public BookService(IBookRepository bookRepository, RedisCacheManager cacheManager)
        {
            _bookRepository = bookRepository;
            _redisCacheManager = cacheManager;
        }

        public List<Book> GetAll()
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
                books = _bookRepository.GetAllBooks();
                foreach (Book book in books)
                {
                    _redisCacheManager.Set($"book:{book.Id}", JsonSerializer.Serialize(book));
                }
            }

            return books;
        }

        public Book Get(int id)
        {
            Book book = null;
            string serializedBook = _redisCacheManager.Get<string>($"book:{id}");

            if (!string.IsNullOrEmpty(serializedBook))
            {
                book = JsonSerializer.Deserialize<Book>(serializedBook);
            }
            else
            {
                book = _bookRepository.GetBook(id);
                if (book != null)
                {
                    _redisCacheManager.Set($"book:{id}", JsonSerializer.Serialize(book));
                }
            }

            return book;
        }

        public Book Add(Book book)
        {
            Book newBook = _bookRepository.InsertBook(book);
            if (newBook != null)
            {
                _redisCacheManager.Set($"book:{newBook.Id}", JsonSerializer.Serialize(newBook));
            }

            return newBook;
        }

        public int Update(UpdateBookEntity book)
        {
            var res = _bookRepository.UpdateBook(book);
            if (res > 0)
            {
                string serializedBook = _redisCacheManager.Get<string>($"book:{book.Id}");
                if (!string.IsNullOrEmpty(serializedBook))
                {
                    Book cachedBook = JsonSerializer.Deserialize<Book>(serializedBook);

                    foreach (var property in book.GetType().GetProperties())
                    {
                        var value = property.GetValue(book);
                        if (value != null)
                        {
                            cachedBook.GetType().GetProperty(property.Name).SetValue(cachedBook, value);
                        }
                    }

                    _redisCacheManager.Set($"book:{cachedBook.Id}", JsonSerializer.Serialize(cachedBook));
                }
            }

            return res;
        }

        public int Delete(int id)
        {
            var res = _bookRepository.DeleteBook(id);
            if (res > 0)
            {
                _redisCacheManager.Remove($"book:{id}");
            }

            return res;
        }

        public List<Book> SearchBooks(string searchTerm)
        {
            return _bookRepository.SearchBooks(searchTerm);
        }
    }
}
