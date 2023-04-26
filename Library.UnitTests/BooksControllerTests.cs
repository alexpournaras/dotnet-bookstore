using WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Model;

namespace Library.UnitTests
{
    public class BooksControllerTests
    {
        private readonly Mock<ILibraryService> _mockLibraryService;
        private readonly BooksController _booksController;

        public BooksControllerTests()
        {
            _mockLibraryService = new Mock<ILibraryService>();
            _booksController = new BooksController(_mockLibraryService.Object);
        }

        private List<Book> GetTestBooks()
        {
            return new List<Book>
            {
                new Book { Id = 1, Date = "2023-04-20", Title = "Test Book 1", Category = "Adventure", Pages = 420, AuthorId = 1 },
                new Book { Id = 2, Date = "2023-01-22", Title = "Test Book 2", Category = "History", Pages = 690, AuthorId = 1 },
                new Book { Id = 3, Date = "2023-01-24", Title = "Test Book 3", Category = "Horror", Pages = 888, AuthorId = 2 }
            };
        }

        [Fact]
        public void GetAllBooks_ShouldReturnAllBooks()
        {
            // Arrange
            var testBooks = GetTestBooks();
            _mockLibraryService.Setup(service => service.GetAllBooks()).Returns(testBooks);

            // Act
            var actionResult = _booksController.GetAllBooks();

            // Assert
            Assert.NotNull(actionResult);
            var books = actionResult.Value;
            Assert.Equal(3, books.Count());
        }

        [Fact]
        public void GetBookById_ShouldReturnBook()
        {
            // Arrange
            int testBookId = 1;
            var testBooks = GetTestBooks();
            _mockLibraryService.Setup(service => service.GetBookById(testBookId)).Returns(testBooks[0]);

            // Act
            var actionResult = _booksController.Get(testBookId);

            // Assert
            Assert.NotNull(actionResult);
            var bookResult = actionResult.Value;
            Assert.Equal(testBookId, bookResult.Id);
        }

        [Fact]
        public void AddBook_ShouldAddBookAndReturnOk()
        {
            // Arrange
            var testBooks = GetTestBooks();
            var book = new Book { Id = 4, Date = "2023-03-26", Title = "Test Book 4", Category = "Funny", Pages = 203, AuthorId = 2 };
            var testAuthor = new Author { Id = 2, FirstName = "Takis", LastName = "Gonias", Country = "Greece" };
            _mockLibraryService.Setup(service => service.GetAuthorById((int)book.AuthorId)).Returns(testAuthor);
            _mockLibraryService.Setup(service => service.AddBook(book)).Verifiable();

            // Act
            var actionResult = _booksController.Post(book);

            // Assert
            _mockLibraryService.Verify();
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Fact]
        public void UpdateBook_ShouldUpdateBookAndReturnOk()
        {
            // Arrange
            int testBookId = 1;
            var testBooks = GetTestBooks();
            var updatedBook = new Book { Id = testBookId, Title = "Test Book 1 Updated", AuthorId = 1 };
            var testAuthor = new Author { Id = 1, FirstName = "Takis", LastName = "Gonias", Country = "Greece" };
            _mockLibraryService.Setup(service => service.GetAuthorById((int)updatedBook.AuthorId)).Returns(testAuthor);
            _mockLibraryService.Setup(service => service.UpdateBook(testBookId, updatedBook)).Returns(testBooks[0]);

            // Act
            var oldBook = testBooks[0];
            oldBook.Author = testAuthor;
            var actionResult = _booksController.Put(testBookId, updatedBook);

            // Assert
            Assert.NotNull(actionResult);
            var okResult = actionResult as OkObjectResult;
            Assert.NotNull(okResult);
        }

        [Fact]
        public void DeleteBook_ShouldDeleteBookAndReturnOk()
        {
            // Arrange
            int testBookId = 1;
            var testBooks = GetTestBooks();
            _mockLibraryService.Setup(service => service.DeleteBook(testBookId)).Returns(testBooks[0]);

            // Act
            var actionResult = _booksController.Delete(testBookId);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Fact]
        public void SearchBooks_ShouldReturnMatchingBooks()
        {
            // Arrange
            var searchTerm = "Test Book 1";
            var testBooks = GetTestBooks();
            var expectedBooks = new List<Book> { testBooks[0] };
            _mockLibraryService.Setup(service => service.FindBooks(searchTerm)).Returns(expectedBooks);

            // Act
            var actionResult = _booksController.SearchBooks(searchTerm);

            // Assert
            Assert.NotNull(actionResult);
            var books = actionResult.Value;
            Assert.Single(books);
            Assert.Equal(searchTerm, books[0].Title);
        }

    }
}
