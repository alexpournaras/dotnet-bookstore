using BookstoreAPI.Models;
using Npgsql;
using System.Text;
using BookstoreAPI.Helpers;


namespace BookstoreAPI.Repositories
{
    public interface IAuthorRepository
    {
        /// <summary>
        /// Get the list of <see cref="Author"/> entities from the database
        /// </summary>
        /// <returns>A list of <see cref="Author"/> entity</returns>
        public List<Author> GetAllAuthors();
        /// <summary>
        /// Get an <see cref="Author"/> from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The <see cref="Author"/> entity</returns>
        public Author GetAuthor(int id);
        /// <summary>
        /// Insert an <see cref="Author"/> into the database
        /// </summary>
        /// <typeparam name="see"></typeparam>
        /// <param name=""></param>
        /// <param name="database"></param>
        /// <returns>The newly created <see cref="Author"/> entity</returns
        public Author InsertAuthor(Author author);
        /// <summary>
        /// Update an <see cref="Author"/> in the database
        /// </summary>
        /// <typeparam name="see"></typeparam>
        /// <param name=""></param>
        /// <param name="database"></param>
        /// <returns>The number of rows affected</returns
        public int UpdateAuthor(UpdateAuthorEntity author);
        /// <summary>
        /// Delete an <see cref="Author"/> from the database
        /// </summary>
        /// <typeparam name="see"></typeparam>
        /// <param name=""></param>
        /// <returns>The number of rows affected</returns
        public int DeleteAuthor(int id);
    }

    public class AuthorRepository : Repository, IAuthorRepository
    {
        public AuthorRepository(DatabaseHelper databaseHelper) : base(databaseHelper) {}

        public List<Author> GetAllAuthors()
        {
            OpenConnection();

            List<Author> authors = new List<Author>();
            const string query = @"
                SELECT author.*, COUNT(book.id) as books_count FROM bookstore.authors author
                LEFT JOIN bookstore.books book ON author.id = book.author_id
                GROUP BY author.id
                ORDER BY author.id";

            using (var cmd = new NpgsqlCommand(query, GetConnection()))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var author = new Author
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            FirstName = Convert.ToString(reader["first_name"]),
                            LastName = Convert.ToString(reader["last_name"]),
                            Country = Convert.ToString(reader["country"]),
                            NumberOfBooks = Convert.ToInt32(reader["books_count"]),
                        };

                        authors.Add(author);
                    }
                }
            }

            CloseConnection();
            return authors;
        }

        public Author GetAuthor(int id)
        {
            OpenConnection();
            const string query = @"
                SELECT author.*, COUNT(book.id) as books_count FROM bookstore.authors author
                LEFT JOIN bookstore.books book ON author.id = book.author_id
                WHERE author.id = @id
                GROUP BY author.id";

            Author author = null;

            using (var cmd = new NpgsqlCommand(query, GetConnection()))
            {
                cmd.Parameters.AddWithValue("id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        author = new Author
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            FirstName = Convert.ToString(reader["first_name"]),
                            LastName = Convert.ToString(reader["last_name"]),
                            Country = Convert.ToString(reader["country"]),
                            NumberOfBooks = Convert.ToInt32(reader["books_count"]),
                        };
                    }
                }
            }

            CloseConnection();
            return author;
        }

        public Author InsertAuthor(Author author)
        {
            OpenConnection();

            const string query = @"
                INSERT INTO bookstore.authors(first_name, last_name, country) 
                VALUES (@first_name, @last_name, @country) RETURNING id";

            using (var cmd = new NpgsqlCommand(query, GetConnection()))
            {
                cmd.Parameters.AddWithValue("first_name", author.FirstName);
                cmd.Parameters.AddWithValue("last_name", author.LastName);
                cmd.Parameters.AddWithValue("country", author.Country);
                author.Id = (int)cmd.ExecuteScalar();
            }

            CloseConnection();
            return author;
        }

        public int UpdateAuthor(UpdateAuthorEntity author)
        {
            OpenConnection();

            StringBuilder query = new StringBuilder("UPDATE bookstore.authors SET ");

            List<NpgsqlParameter> paramList = new List<NpgsqlParameter>();

            if (author.FirstName != null)
            {
                query.Append("first_name = @first_name, ");
                paramList.Add(new NpgsqlParameter("first_name", author.FirstName));
            }
            if (author.LastName != null)
            {
                query.Append("last_name = @last_name, ");
                paramList.Add(new NpgsqlParameter("last_name", author.LastName));
            }
            if (author.Country != null)
            {
                query.Append("country = @country, ");
                paramList.Add(new NpgsqlParameter("country", author.Country));
            }

            query = query.Remove(query.Length - 2, 2); // Remove comma and space

            query.Append($" WHERE id = @id");
            paramList.Add(new NpgsqlParameter("id", author.Id));

            int res;
            using (var cmd = new NpgsqlCommand(query.ToString(), GetConnection()))
            {
                cmd.Parameters.AddRange(paramList.ToArray());
                res = cmd.ExecuteNonQuery();

            }

            CloseConnection();
            return res;
        }

        public int DeleteAuthor(int id)
        {
            OpenConnection();
            const string query = "DELETE FROM bookstore.authors WHERE id = @id";
            int res;

            try
            {
                using (var cmd = new NpgsqlCommand(query, GetConnection()))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    res = cmd.ExecuteNonQuery();
                }
            }
            catch (PostgresException ex) when (ex.SqlState == "23503")
            {
                // update or delete on table "authors" violates foreign key constraint "fk_author_id" on table "books"
                // Foreign key constraint violation
                res = -1;
            }

            CloseConnection();
            return res;
        }
    }
}
