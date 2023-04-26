using WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Model;

namespace Library.UnitTests
{
    public class AuthorsControllerTests
    {
        private readonly Mock<ILibraryService> _mockLibraryService;
        private readonly AuthorsController _authorsController;

        public AuthorsControllerTests()
        {
            _mockLibraryService = new Mock<ILibraryService>();
            _authorsController = new AuthorsController(_mockLibraryService.Object);
        }

        private List<Author> GetTestAuthors()
        {
            return new List<Author>
            {
                new Author { Id = 1, FirstName = "Alex", LastName = "Pourna", Country = "Greece" },
                new Author { Id = 2, FirstName = "Giannis", LastName = "Deme", Country = "Italy" },
                new Author { Id = 3, FirstName = "Takis", LastName = "Gonias", Country = "Austria" }
            };
        }

        [Fact]
        public void GetAllAuthors_ShouldReturnAllAuthors()
        {
            // Arrange
            var testAuthors = GetTestAuthors();
            _mockLibraryService.Setup(service => service.GetAllAuthors()).Returns(testAuthors);

            // Act
            var actionResult = _authorsController.GetAllAuthors();

            // Assert
            Assert.NotNull(actionResult);
            var authors = actionResult.Value;
            Assert.Equal(3, authors.Count());
        }

        [Fact]
        public void GetAuthorById_ShouldReturnAuthor()
        {
            // Arrange
            int testAuthorId = 1;
            var testAuthors = GetTestAuthors();
            _mockLibraryService.Setup(service => service.GetAuthorById(testAuthorId)).Returns(testAuthors[0]);

            // Act
            var actionResult = _authorsController.Get(testAuthorId);

            // Assert
            Assert.NotNull(actionResult);
            var authorResult = actionResult.Value;
            Assert.Equal(testAuthorId, authorResult.Id);
        }

        [Fact]
        public void AddAuthor_ShouldAddAuthorAndReturnOk()
        {
            // Arrange
            var testAuthors = GetTestAuthors();
            var newAuthor = new Author { Id = 4, FirstName = "Jim", LastName = "A.", Country = "Spain" };
            _mockLibraryService.Setup(service => service.AddAuthor(newAuthor)).Verifiable();

            // Act
            var actionResult = _authorsController.Post(newAuthor);

            // Assert
            _mockLibraryService.Verify();
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Fact]
        public void UpdateAuthor_ShouldUpdateAuthorAndReturnOk()
        {
            // Arrange
            int testAuthorId = 1;
            var testAuthors = GetTestAuthors();
            var updatedAuthor = new Author { Id = testAuthorId, FirstName = "Jim", LastName = "A.", Country = "Spain" };
            _mockLibraryService.Setup(service => service.UpdateAuthor(testAuthorId, updatedAuthor)).Returns(testAuthors[0]);

            // Act
            var actionResult = _authorsController.Put(testAuthorId, updatedAuthor);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Fact]
        public void DeleteAuthor_ShouldDeleteAuthorAndReturnOk()
        {
            // Arrange
            int testAuthorId = 1;
            var testAuthors = GetTestAuthors();
            _mockLibraryService.Setup(service => service.GetBooksByAuthor(testAuthorId)).Returns(new List<Book>());
            _mockLibraryService.Setup(service => service.DeleteAuthor(testAuthorId)).Returns(testAuthors[0]);

            // Act
            var actionResult = _authorsController.Delete(testAuthorId);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult);
        }
    }
}

