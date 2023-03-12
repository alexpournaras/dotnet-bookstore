using Npgsql;
using WebAPI.Model;

namespace WebAPI.Datalayer
{
    public class AuthorsDatalayer
    {
        private string _postgresHost = "localhost";
        private string _postgresDbName = "library";
        private string _postgresDbUser = "postgres";
        private string _postgresDbPassword = "password";

        NpgsqlConnection _dbInstance;

        public AuthorsDatalayer()
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
    }
}