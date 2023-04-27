using BookstoreAPI.Models;
using BookstoreAPI.Repositories;
using BookstoreAPI.Helpers;

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
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public List<Book> GetAll()
        {
            return _bookRepository.GetAllBooks();
        }

        public Book Get(int id)
        {
            return _bookRepository.GetBook(id);
        }

        public Book Add(Book book)
        {
            return _bookRepository.InsertBook(book);
        }

        public int Update(UpdateBookEntity book)
        {
            return _bookRepository.UpdateBook(book);
        }

        public int Delete(int id)
        {
            return _bookRepository.DeleteBook(id);
        }

        public List<Book> SearchBooks(string searchTerm)
        {
            return _bookRepository.SearchBooks(searchTerm);
        }
    }
}
