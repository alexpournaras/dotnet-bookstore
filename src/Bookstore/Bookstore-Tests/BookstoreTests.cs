using Xunit;
using Moq;
using BookstoreAPI.Controllers;
using BookstoreAPI.Services;
using BookstoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BookstoreAPI;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Formatting;

namespace BookstoreTests
{
    public class BookstoreTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        private readonly string _jwtToken;

        public BookstoreTests(TestFixture fixture)
        {
            _fixture = fixture;
            _configuration = fixture._configuration;
    
            var testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseEnvironment("Test")
                .UseConfiguration(_fixture._configuration)
                .UseStartup<Startup>());

            _client = testServer.CreateClient();
            
            _jwtToken = JwtTokenGenerator.GenerateToken(_configuration, "Developer");
        }
        
        [Fact]
        public async Task AuthorsTest()
        {
            // GetAll
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
            var response = await _client.GetAsync("/api/authors");
            Assert.True(response.IsSuccessStatusCode);
            
            var authors = await response.Content.ReadAsAsync<List<Author>>();
            Assert.NotEmpty(authors);
            Assert.Equal(3, authors.Count());
            
            // Get
            int authorId = 1;
            response = await _client.GetAsync("/api/authors/" + authorId);
            Assert.True(response.IsSuccessStatusCode);
            
            Author author = await response.Content.ReadAsAsync<Author>();
            Assert.NotNull(author);
            Assert.Equal("Greece", author.Country);
            
            // Get but not found
            authorId = 4;
            response = await _client.GetAsync("/api/authors/" + authorId);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            
            // Create
            var newAuthor = new { first_name = "Teo", last_name = "Pourna", country = "Zimbabwe" };
            response = await _client.PostAsJsonAsync("/api/authors", newAuthor);
            Assert.True(response.IsSuccessStatusCode);
            
            var createdAuthor = await response.Content.ReadAsAsync<Author>();
            Assert.NotNull(createdAuthor);
            Assert.Equal("Zimbabwe", createdAuthor.Country);
            
            // Update
            var updateAuthor = new UpdateAuthorEntity { Id = 1, FirstName = "Teo" };
            response = await _client.PatchAsJsonAsync("/api/authors", updateAuthor);
            Assert.True(response.IsSuccessStatusCode);

            var result = await response.Content.ReadAsAsync<Dictionary<string, int>>();
            Assert.NotNull(result);
            Assert.True(result.ContainsKey("rows_affected"));
            Assert.Equal(1, result["rows_affected"]);
            
            // Update but not found
            updateAuthor = new UpdateAuthorEntity { Id = 999, FirstName = "Teo" };
            response = await _client.PatchAsJsonAsync("/api/authors", updateAuthor);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            
            // Delete
            authorId = 3;
            response = await _client.DeleteAsync("/api/authors/" + authorId);
            Assert.True(response.IsSuccessStatusCode);
            
            result = await response.Content.ReadAsAsync<Dictionary<string, int>>();
            Assert.NotNull(result);
            Assert.True(result.ContainsKey("rows_affected"));
            Assert.Equal(1, result["rows_affected"]);
            
            // Delete but not found
            authorId = 999;
            response = await _client.DeleteAsync("/api/authors/" + authorId);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            
            // Delete but cannot delete author
            authorId = 1;
            response = await _client.DeleteAsync("/api/authors/" + authorId);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [Fact]
        public async Task BooksTest()
        {
            // GetAll
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
            var response = await _client.GetAsync("/api/books");
            Assert.True(response.IsSuccessStatusCode);

            var books = await response.Content.ReadAsAsync<List<Book>>();
            Assert.NotEmpty(books);
            Assert.Equal(3, books.Count);
            
            // Get
            int bookId = 1;
            response = await _client.GetAsync("/api/books/" + bookId);
            Assert.True(response.IsSuccessStatusCode);

            var book = await response.Content.ReadAsAsync<Book>();
            Assert.Equal("Test Book 1", book.Title);
            
            // Get but not found
            bookId = 999;
            response = await _client.GetAsync("/api/books/" + bookId);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            
            // Create
            var newBook = new { title = "New Test Book", date = DateTime.Now, category = "Funny", pages = 420, author_id = 2 };
            response = await _client.PostAsJsonAsync("/api/books", newBook);
            Assert.True(response.IsSuccessStatusCode);

            var createdBook = await response.Content.ReadAsAsync<Book>();
            Assert.NotNull(createdBook);
            Assert.Equal("New Test Book", createdBook.Title);
            
            // Create but cannot create book
            newBook = new { title = "New Test Book", date = DateTime.Now, category = "Funny", pages = 420, author_id = 999 };
            response = await _client.PostAsJsonAsync("/api/books", newBook);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            
            // Update
            var updateBook = new UpdateBookEntity { Id = 1, Title = "Updated Test Book 1" };
            response = await _client.PatchAsJsonAsync("/api/books", updateBook);
            Assert.True(response.IsSuccessStatusCode);

            var result = await response.Content.ReadAsAsync<Dictionary<string, int>>();
            Assert.NotNull(result);
            Assert.True(result.ContainsKey("rows_affected"));
            Assert.Equal(1, result["rows_affected"]);
            
            // Update but not found
            updateBook = new UpdateBookEntity { Id = 999, Title = "Updated Test Book 1" };
            response = await _client.PatchAsJsonAsync("/api/books", updateBook);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            
            // Update but cannot update book
            updateBook = new UpdateBookEntity { Id = 1, AuthorId = 999};
            response = await _client.PatchAsJsonAsync("/api/books", updateBook);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            // Delete
            bookId = 3;
            response = await _client.DeleteAsync($"/api/books/{bookId}");
            Assert.True(response.IsSuccessStatusCode);

            result = await response.Content.ReadAsAsync<Dictionary<string, int>>();
            Assert.NotNull(result);
            Assert.True(result.ContainsKey("rows_affected"));
            Assert.Equal(1, result["rows_affected"]);
            
            // Delete but not found
            bookId = 999;
            response = await _client.DeleteAsync($"/api/books/{bookId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            
            // Search for book attributes
            string searchTerm = "Test";
            response = await _client.GetAsync($"/api/books/search?searchTerm={searchTerm}");
            Assert.True(response.IsSuccessStatusCode);

            books = await response.Content.ReadAsAsync<List<Book>>();
            Assert.NotEmpty(books);
            Assert.Equal(3, books.Count);
            
            // Search for author attributes
            searchTerm = "Giannis";
            response = await _client.GetAsync($"/api/books/search?searchTerm={searchTerm}");
            Assert.True(response.IsSuccessStatusCode);

            books = await response.Content.ReadAsAsync<List<Book>>();
            Assert.NotEmpty(books);
            Assert.Equal(1, books.Count);
        }
        
        [Fact]
        public async Task JobsTest()
        {
            // Parse books job
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
            var books = new List<UpdateBookEntity> {
                new UpdateBookEntity { Id = 1, Title = "Updated Test Book 1" },
                new UpdateBookEntity { Id = 2, Title = "Updated Test Book 2" },
                new UpdateBookEntity { Title = "New Test Book From Jobs", Date = DateTime.Now, Category = "Adventure", Pages = 690, AuthorId = 1 }
            };
            
            var response = await _client.PostAsJsonAsync("/api/parse/books", books);
            Assert.True(response.IsSuccessStatusCode);

            var result = await response.Content.ReadAsAsync<Dictionary<string, object>>();
            Assert.NotNull(result);
            Assert.True(result.ContainsKey("id"));
            Assert.True(result.ContainsKey("status"));
            Assert.Equal("Queued", result["status"].ToString());
            
            // Wait job to finish
            Thread.Sleep(5000);
            
            // Get job status
            string jobId = result["id"].ToString();
            response = await _client.GetAsync($"/api/jobs/{jobId}");
            Assert.True(response.IsSuccessStatusCode);

            result = await response.Content.ReadAsAsync<Dictionary<string, object>>();
            Assert.NotNull(result);
            Assert.True(result.ContainsKey("id"));
            Assert.True(result.ContainsKey("status"));
            Assert.Equal("Completed", result["status"].ToString());

        }
        
        #region standaloneTests

        // [Fact]
        // public async Task AuthorGetAll()
        // {
        //     _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
        //
        //     var response = await _client.GetAsync("/api/authors");
        //     
        //     Assert.True(response.IsSuccessStatusCode);
        //     
        //     var authors = await response.Content.ReadAsAsync<List<Author>>();
        //     Assert.NotEmpty(authors);
        //     Assert.Equal(3, authors.Count());
        // }
        //
        // [Fact]
        // public async Task AuthorGet()
        // {
        //     int authorId = 1;
        //     var response = await _client.GetAsync("/api/authors/" + authorId);
        //     
        //     Assert.True(response.IsSuccessStatusCode);
        //     
        //     Author author = await response.Content.ReadAsAsync<Author>();
        //     Assert.NotNull(author);
        //     Assert.Equal("Greece", author.Country);
        // }
        //
        // [Fact]
        // public async Task AuthorAdd()
        // {
        //     _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
        //     var newAuthor = new { first_name = "Teo", last_name = "Pourna", country = "Zimbabwe" };
        //
        //     var response = await _client.PostAsJsonAsync("/api/authors", newAuthor);
        //
        //     Assert.True(response.IsSuccessStatusCode);
        //
        //     var createdAuthor = await response.Content.ReadAsAsync<Author>();
        //     Assert.NotNull(createdAuthor);
        //     Assert.Equal("Zimbabwe", createdAuthor.Country);
        // }
        //
        // [Fact]
        // public async Task AuthorUpdate()
        // {
        //     _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
        //     var updateAuthor = new UpdateAuthorEntity { Id = 1, FirstName = "Teo" };
        //
        //     var response = await _client.PatchAsJsonAsync("/api/authors", updateAuthor);
        //
        //     Assert.True(response.IsSuccessStatusCode);
        //
        //     var result = await response.Content.ReadAsAsync<Dictionary<string, int>>();
        //     Assert.NotNull(result);
        //     Assert.True(result.ContainsKey("rows_affected"));
        //     Assert.Equal(1, result["rows_affected"]);
        // }
        //
        // [Fact]
        // public async Task AuthorDelete()
        // {
        //     _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
        //     int authorId = 3;
        //
        //     var response = await _client.DeleteAsync("/api/authors/" + authorId);
        //
        //     Assert.True(response.IsSuccessStatusCode);
        //     
        //     var result = await response.Content.ReadAsAsync<Dictionary<string, int>>();
        //     Assert.NotNull(result);
        //     Assert.True(result.ContainsKey("rows_affected"));
        //     Assert.Equal(1, result["rows_affected"]);
        // }
        //
        // [Fact]
        // public async Task BookGetAll()
        // {
        //     _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
        //
        //     var response = await _client.GetAsync("/api/books");
        //
        //     Assert.True(response.IsSuccessStatusCode);
        //
        //     var books = await response.Content.ReadAsAsync<List<Book>>();
        //     Assert.NotEmpty(books);
        //     Assert.Equal(3, books.Count);
        // }
        //
        // [Fact]
        // public async Task BookGet()
        // {
        //     int bookId = 1;
        //     var response = await _client.GetAsync($"/api/books/{bookId}");
        //
        //     Assert.True(response.IsSuccessStatusCode);
        //
        //     var book = await response.Content.ReadAsAsync<Book>();
        //     Assert.Equal("Test Book 1", book.Title);
        // }
        //
        // [Fact]
        // public async Task BookAdd()
        // {
        //     _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
        //     var newBook = new { title = "New Test Book", date = DateTime.Now, category = "Funny", pages = 420, author_id = 2 };
        //
        //     var response = await _client.PostAsJsonAsync("/api/books", newBook);
        //     Assert.True(response.IsSuccessStatusCode);
        //
        //     var createdBook = await response.Content.ReadAsAsync<Book>();
        //     Assert.Equal("New Test Book", createdBook.Title);
        // }
        //
        // [Fact]
        // public async Task BookUpdate()
        // {
        //     _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
        //     var updateBook = new UpdateBookEntity { Id = 1, Title = "Updated Test Book 1" };
        //     
        //     var response = await _client.PatchAsJsonAsync("/api/books", updateBook);
        //     Assert.True(response.IsSuccessStatusCode);
        //
        //     var result = await response.Content.ReadAsAsync<Dictionary<string, int>>();
        //     Assert.NotNull(result);
        //     Assert.True(result.ContainsKey("rows_affected"));
        //     Assert.Equal(1, result["rows_affected"]);
        // }
        //
        // [Fact]
        // public async Task BookDelete()
        // {
        //     _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
        //     int bookId = 3;
        //
        //     var response = await _client.DeleteAsync($"/api/books/{bookId}");
        //     response.EnsureSuccessStatusCode();
        //
        //     var result = await response.Content.ReadAsAsync<Dictionary<string, int>>();
        //     Assert.NotNull(result);
        //     Assert.True(result.ContainsKey("rows_affected"));
        //     Assert.Equal(1, result["rows_affected"]);
        // }
        //
        // [Fact]
        // public async Task BookSearch()
        // {
        //     _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
        //     string searchTerm = "Test";
        //
        //     var response = await _client.GetAsync($"/api/books/search?searchTerm={searchTerm}");
        //     Assert.True(response.IsSuccessStatusCode);
        //
        //     var books = await response.Content.ReadAsAsync<List<Book>>();
        //     Assert.NotEmpty(books);
        //     Assert.Equal(3, books.Count);
        // }
        
        #endregion standaloneTests
        
    }
    
    
}

