using Npgsql;
using NpgsqlTypes;
using WebAPI.Model;
using System.Globalization;

namespace WebAPI.Datalayer
{
    public class LibraryDatalayer
    {
        private string _postgresHost = "localhost";
        private string _postgresDbName = "library";
        private string _postgresDbUser = "postgres";
        private string _postgresDbPassword = "password";

        NpgsqlConnection _dbInstance;

        public LibraryDatalayer()
        {
            _dbInstance = new NpgsqlConnection(GetConnString());
        }

        private string GetConnString()
        {
            return String.Format("host={0};username={1};password={2};database={3}", _postgresHost, _postgresDbUser, _postgresDbPassword, _postgresDbName);
        }

        public void OpenConnection()
        {
            _dbInstance.Open();
        }
        public void CloseConnection()
        {
            _dbInstance.Close();
        }

        public List<Author> GetAllAuthors()
        {
            List<Author> authors = new List<Author>();

            string query = "SELECT *, library.books.id as book_id FROM library.authors LEFT JOIN library.books ON library.authors.id = library.books.author_id ORDER BY library.authors.id";

            using (var cmd = new NpgsqlCommand(query, _dbInstance))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Author author = authors.FirstOrDefault(author => author.Id == Convert.ToInt32(reader["id"]));
                        if (author == null)
                        {
                            author = new Author();
                            author.Id = Convert.ToInt32(reader["id"]);
                            author.FirstName = Convert.ToString(reader["first_name"]);
                            author.LastName = Convert.ToString(reader["last_name"]);
                            author.Country = Convert.ToString(reader["country"]);
                            author.Books = new List<Book>();

                            authors.Add(author);
                        }

                        if (reader["book_id"] != DBNull.Value)
                        {
                            Book book = new Book();
                            book.Id = Convert.ToInt32(reader["book_id"]);
                            book.Date = Convert.ToString(reader["date"]);
                            book.Title = Convert.ToString(reader["title"]);
                            book.Category = Convert.ToString(reader["category"]);
                            book.Pages = Convert.ToInt32(reader["pages"]);
                            book.AuthorId = Convert.ToInt32(reader["author_id"]);

                            author.Books.Add(book);
                        }
                    }
                }
            }

            return authors;
        }

        public Author GetAuthorById(int id)
        {
            Author author = null;

            string query = "SELECT *, library.books.id as book_id FROM library.authors LEFT JOIN library.books ON library.authors.id = library.books.author_id WHERE library.authors.id = @Id";

            using (var cmd = new NpgsqlCommand(query, _dbInstance))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (author == null)
                        {
                            author = new Author();
                            author.Id = Convert.ToInt32(reader["id"]);
                            author.FirstName = Convert.ToString(reader["first_name"]);
                            author.LastName = Convert.ToString(reader["last_name"]);
                            author.Country = Convert.ToString(reader["country"]);
                            author.Books = new List<Book>();
                        }

                        if (reader["book_id"] != DBNull.Value)
                        {
                            Book book = new Book();
                            book.Id = Convert.ToInt32(reader["book_id"]);
                            book.Date = Convert.ToString(reader["date"]);
                            book.Title = Convert.ToString(reader["title"]);
                            book.Category = Convert.ToString(reader["category"]);
                            book.Pages = Convert.ToInt32(reader["pages"]);
                            book.AuthorId = Convert.ToInt32(reader["author_id"]);

                            author.Books.Add(book);
                        }
                    }
                }
            }

            return author;
        }

        public void AddAuthor(Author author)
        {
            string query = "INSERT INTO library.authors (first_name, last_name, country) VALUES (@FirstName, @LastName, @Country)";

            using (var cmd = new NpgsqlCommand(query, _dbInstance))
            {
                cmd.Parameters.AddWithValue("@FirstName", author.FirstName);
                cmd.Parameters.AddWithValue("@LastName", author.LastName);
                cmd.Parameters.AddWithValue("@Country", author.Country);

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateAuthor(Author existingAuthor, Author updatedAuthor)
        {
            string query = "UPDATE library.authors SET first_name = @FirstName, last_name = @LastName, country = @Country WHERE id = @Id";

            using (var cmd = new NpgsqlCommand(query, _dbInstance))
            {
                cmd.Parameters.AddWithValue("@Id", existingAuthor.Id);
                cmd.Parameters.AddWithValue("@FirstName", updatedAuthor.FirstName);
                cmd.Parameters.AddWithValue("@LastName", updatedAuthor.LastName);
                cmd.Parameters.AddWithValue("@Country", updatedAuthor.Country);

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteAuthor(Author author)
        {
            string query = "DELETE FROM library.authors WHERE id = @Id";

            using (var cmd = new NpgsqlCommand(query, _dbInstance))
            {
                cmd.Parameters.AddWithValue("@Id", author.Id);

                cmd.ExecuteNonQuery();
            }
        }

        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();

            string query = "SELECT * FROM library.books INNER JOIN library.authors ON library.books.author_id = library.authors.id ORDER BY library.books.id";

            using (var cmd = new NpgsqlCommand(query, _dbInstance))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Author author = new Author();
                        author.Id = Convert.ToInt32(reader["author_id"]);
                        author.FirstName = Convert.ToString(reader["first_name"]);
                        author.LastName = Convert.ToString(reader["last_name"]);
                        author.Country = Convert.ToString(reader["country"]);

                        Book book = new Book();
                        book.Id = Convert.ToInt32(reader["id"]);
                        book.Date = Convert.ToString(reader["date"]);
                        book.Title = Convert.ToString(reader["title"]);
                        book.Category = Convert.ToString(reader["category"]);
                        book.Pages = Convert.ToInt32(reader["pages"]);
                        book.AuthorId = Convert.ToInt32(reader["author_id"]);
                        book.Author = author;

                        books.Add(book);
                    }
                }
            }

            return books;
        }

        public Book GetBookById(int id)
        {
            string query = "SELECT * FROM library.books INNER JOIN library.authors ON library.books.author_id = library.authors.id WHERE library.books.id = @Id";

            using (var cmd = new NpgsqlCommand(query, _dbInstance))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Author author = new Author();
                        author.Id = Convert.ToInt32(reader["author_id"]);
                        author.FirstName = Convert.ToString(reader["first_name"]);
                        author.LastName = Convert.ToString(reader["last_name"]);
                        author.Country = Convert.ToString(reader["country"]);

                        Book book = new Book();
                        book.Id = Convert.ToInt32(reader["id"]);
                        book.Date = Convert.ToString(reader["date"]);
                        book.Title = Convert.ToString(reader["title"]);
                        book.Category = Convert.ToString(reader["category"]);
                        book.Pages = Convert.ToInt32(reader["pages"]);
                        book.AuthorId = Convert.ToInt32(reader["author_id"]);
                        book.Author = author;

                        return book;
                    }
                }
            }

            return null;
        }

        public List<Book> GetBooksByAuthor(int authorId)
        {
            List<Book> books = new List<Book>();

            string query = "SELECT * FROM library.books WHERE author_id = @AuthorId";

            using (var cmd = new NpgsqlCommand(query, _dbInstance))
            {
                cmd.Parameters.AddWithValue("@AuthorId", authorId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Book book = new Book();
                        book.Id = Convert.ToInt32(reader["id"]);
                        book.Date = Convert.ToString(reader["date"]);
                        book.Title = Convert.ToString(reader["title"]);
                        book.Category = Convert.ToString(reader["category"]);
                        book.Pages = Convert.ToInt32(reader["pages"]);
                        book.AuthorId = Convert.ToInt32(reader["author_id"]);

                        books.Add(book);
                    }
                }
            }

            return books;
        }

        public void AddBook(Book book)
        {
            string query = "INSERT INTO library.books (date, author_id, title, category, pages) VALUES (@Date, @AuthorId, @Title, @Category, @Pages)";

            using (var cmd = new NpgsqlCommand(query, _dbInstance))
            {
                cmd.Parameters.AddWithValue("@Date", NpgsqlDbType.Date, DateTime.ParseExact(book.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@AuthorId", book.AuthorId);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Category", book.Category);
                cmd.Parameters.AddWithValue("@Pages", book.Pages);

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateBook(Book existingBook, Book updatedBook)
        {
            string query = "UPDATE library.books SET date = @Date, author_id = @AuthorId, title = @Title, category = @Category, pages = @Pages WHERE id = @Id";

            using (var cmd = new NpgsqlCommand(query, _dbInstance))
            {
                cmd.Parameters.AddWithValue("@Date", NpgsqlDbType.Date, DateTime.ParseExact(updatedBook.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@Id", existingBook.Id);
                cmd.Parameters.AddWithValue("@AuthorId", updatedBook.AuthorId);
                cmd.Parameters.AddWithValue("@Title", updatedBook.Title);
                cmd.Parameters.AddWithValue("@Category", updatedBook.Category);
                cmd.Parameters.AddWithValue("@Pages", updatedBook.Pages);

                cmd.ExecuteNonQuery();
            }
        }

        public void InsertOrUpdateBook(Book book)
        {
            string query = "";

            if (book.Id == 0) {
                query = "INSERT INTO library.books (date, author_id, title, category, pages) VALUES (@Date, @AuthorId, @Title, @Category, @Pages)";
            } else {
                query = "UPDATE library.books SET date = @Date, author_id = @AuthorId, title = @Title, category = @Category, pages = @Pages WHERE id = @Id";
            }

            lock (_dbInstance)
            {
                using (var cmd = new NpgsqlCommand(query, _dbInstance))
                {
                    cmd.Parameters.AddWithValue("@Date", NpgsqlDbType.Date, DateTime.ParseExact(book.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@Id", book.Id);
                    cmd.Parameters.AddWithValue("@AuthorId", book.AuthorId);
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@Category", book.Category);
                    cmd.Parameters.AddWithValue("@Pages", book.Pages);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteBook(Book book)
        {
            string query = "DELETE FROM library.books WHERE id = @Id";

            using (var cmd = new NpgsqlCommand(query, _dbInstance))
            {
                cmd.Parameters.AddWithValue("@Id", book.Id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}