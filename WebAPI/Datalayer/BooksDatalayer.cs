using Npgsql;
using NpgsqlTypes;
using System.Globalization;
using WebAPI.Model;

namespace WebAPI.Datalayer
{
    public class BooksDatalayer
    {
        private string _postgresHost = "localhost";
        private string _postgresDbName = "library";
        private string _postgresDbUser = "postgres";
        private string _postgresDbPassword = "password";

        NpgsqlConnection _dbInstance;

        public BooksDatalayer()
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
                        author.Books = Convert.ToInt32(reader["books"]);

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
                        author.Books = Convert.ToInt32(reader["books"]);

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