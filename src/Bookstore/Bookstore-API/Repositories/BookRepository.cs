using BookstoreAPI.Models;
using Npgsql;
using System.Text;
using BookstoreAPI.Helpers;

namespace BookstoreAPI.Repositories
{
    public interface IBookRepository
    {
        /// <summary>
        /// Get the list of <see cref="Book"/> entities from the database
        /// </summary>
        /// <returns>A list of <see cref="Book"/> entity</returns>
        public List<Book> GetAllBooks();
        /// <summary>
        /// Get a <see cref="Book"/> from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The <see cref="Book"/> entities</returns>
        public Book GetBook(int id);
        /// <summary>
        /// Insert a <see cref="Book"/> into the database
        /// </summary>
        /// <param name="book"></param>
        /// <returns>The newly created <see cref="Book"/> entity</returns>
        public Book InsertBook(Book book);
         /// <summary>
        /// Update a <see cref="Book"/> in the database
        /// </summary>
        /// <param name="book"></param>
        /// <returns>The number of rows affected</returns>
        public int UpdateBook(UpdateBookEntity book);
        /// <summary>
        /// Update or insert a <see cref="Book"/> in the database
        /// </summary>
        /// <param name="book"></param>
        /// <returns>The number of rows affected</returns>
        public int UpsertBook(UpdateBookEntity book);
        /// <summary>
        /// Delete a <see cref="Book"/> from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The number of rows affected</returns>
        public int DeleteBook(int id);
        /// <summary>
        /// Get the list of <see cref="Book"/> entities from the database
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns>A list of <see cref="Book"/> entity</returns>
        public List<Book> SearchBooks(string searchTerm);

    }
    public class BookRepository : Repository, IBookRepository
    {
        public BookRepository(DatabaseHelper databaseHelper) : base(databaseHelper) {}

        public List<Book> GetAllBooks()
        {
            OpenConnection();

            List<Book> books = new List<Book>();
            const string query = "SELECT * FROM bookstore.books";

            using (var cmd = new NpgsqlCommand(query, GetConnection()))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var book = new Book
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Date = Convert.ToDateTime(reader["date"]),
                            Title = Convert.ToString(reader["title"]),
                            Category = Convert.ToString(reader["category"]),
                            Pages = Convert.ToInt32(reader["pages"]),
                            AuthorId = Convert.ToInt32(reader["author_id"]),
                        };

                        books.Add(book);
                    }
                }
            }

            CloseConnection();
            return books;
        }

        public Book GetBook(int id)
        {
            OpenConnection();
            const string query = "SELECT * FROM bookstore.books WHERE id = @id";
            Book book = null;

            using (var cmd = new NpgsqlCommand(query, GetConnection()))
            {
                cmd.Parameters.AddWithValue("id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        book = new Book
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Date = Convert.ToDateTime(reader["date"]),
                            Title = Convert.ToString(reader["title"]),
                            Category = Convert.ToString(reader["category"]),
                            Pages = Convert.ToInt32(reader["pages"]),
                            AuthorId = Convert.ToInt32(reader["author_id"]),
                        };
                    }
                }
            }

            CloseConnection();
            return book;
        }

        public Book InsertBook(Book book)
        {
            OpenConnection();
            const string query = @"
                INSERT INTO bookstore.books(title, date, category, pages, author_id)
                VALUES (@title, @date, @category, @pages, @author_id) RETURNING id";

            try
            {
                using (var cmd = new NpgsqlCommand(query, GetConnection()))
                {
                    cmd.Parameters.AddWithValue("title", book.Title);
                    cmd.Parameters.AddWithValue("date", book.Date);
                    cmd.Parameters.AddWithValue("category", book.Category);
                    cmd.Parameters.AddWithValue("pages", book.Pages);
                    cmd.Parameters.AddWithValue("author_id", book.AuthorId);
                    book.Id = (int)cmd.ExecuteScalar();
                }
            }
            catch (PostgresException ex) when (ex.SqlState == "23503")
            {
                // insert or update on table "books" violates foreign key constraint "fk_author_id"
                // Foreign key constraint violation
                book = null;
            }

            CloseConnection();
            return book;
        }

        public int UpdateBook(UpdateBookEntity book)
        {
            OpenConnection();

            StringBuilder query = new StringBuilder("UPDATE bookstore.books SET ");

            List<NpgsqlParameter> paramList = new List<NpgsqlParameter>();

            if (book.Title != null)
            {
                query.Append("title = @title, ");
                paramList.Add(new NpgsqlParameter("title", book.Title));
            }
            if (book.Date != null)
            {
                query.Append("date = @date, ");
                paramList.Add(new NpgsqlParameter("date", book.Date));
            }
            if (book.Category != null)
            {
                query.Append("category = @category, ");
                paramList.Add(new NpgsqlParameter("category", book.Category));
            }
            if (book.Pages != null)
            {
                query.Append("pages = @pages, ");
                paramList.Add(new NpgsqlParameter("pages", book.Pages));
            }
            if (book.AuthorId != null)
            {
                query.Append("author_id = @author_id, ");
                paramList.Add(new NpgsqlParameter("author_id", book.AuthorId));
            }

            query = query.Remove(query.Length - 2, 2); // Remove comma and space

            query.Append($" WHERE id = @id");
            paramList.Add(new NpgsqlParameter("id", book.Id));
            int res;

            try
            {
                using (var cmd = new NpgsqlCommand(query.ToString(), GetConnection()))
                {
                    cmd.Parameters.AddRange(paramList.ToArray());
                    res = cmd.ExecuteNonQuery();
                }
            }
            catch (PostgresException ex) when (ex.SqlState == "23503")
            {
                // insert or update on table "books" violates foreign key constraint "fk_author_id"
                // Foreign key constraint violation
                res = -1;
            }

            CloseConnection();
            return res;
        }

        public int UpsertBook(UpdateBookEntity book)
        {
            OpenConnection();
            int res = 0;

            const string query = @"
                INSERT INTO bookstore.books(id, title, date, category, pages, author_id)
                VALUES (@id, @title, @date, @category, @pages, @author_id)
                ON CONFLICT (id) DO UPDATE SET
                    title = @title,
                    date = @date,
                    category = @category,
                    pages = @pages,
                    author_id = @author_id";

            try
            {
                using (var cmd = new NpgsqlCommand(query, GetConnection()))
                {
                    cmd.Parameters.AddWithValue("id", book.Id);
                    cmd.Parameters.AddWithValue("title", book.Title ?? default);
                    cmd.Parameters.AddWithValue("date", book.Date ?? default);
                    cmd.Parameters.AddWithValue("category", book.Category ?? default);
                    cmd.Parameters.AddWithValue("pages", book.Pages ?? default);
                    cmd.Parameters.AddWithValue("author_id", book.AuthorId ?? default);
                    res = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                CloseConnection();
            }

            CloseConnection();
            return res;
        }

        public int DeleteBook(int id)
        {
            OpenConnection();
            const string query = "DELETE FROM bookstore.books WHERE id = @id";

            int res;
            using (var cmd = new NpgsqlCommand(query, GetConnection()))
            {
                cmd.Parameters.AddWithValue("id", id);
                res = cmd.ExecuteNonQuery();
            }

            CloseConnection();
            return res;
        }

        public List<Book> SearchBooks(string searchTerm)
        {
            OpenConnection();
            List<Book> books = new List<Book>();

            string query = "SELECT * FROM bookstore.books INNER JOIN bookstore.authors ON bookstore.books.author_id = bookstore.authors.id "
                + "WHERE bookstore.books.title LIKE '%" + searchTerm + "%' "
                + "OR bookstore.books.category LIKE '%" + searchTerm + "%' "
                + "OR bookstore.authors.first_name LIKE '%" + searchTerm + "%' "
                + "OR bookstore.authors.last_name LIKE '%" + searchTerm + "%' "
                + "OR bookstore.authors.country LIKE '%" + searchTerm + "%' "
                + "ORDER BY bookstore.books.id";

            using (var cmd = new NpgsqlCommand(query, GetConnection()))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var book = new Book
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Date = Convert.ToDateTime(reader["date"]),
                            Title = Convert.ToString(reader["title"]),
                            Category = Convert.ToString(reader["category"]),
                            Pages = Convert.ToInt32(reader["pages"]),
                            AuthorId = Convert.ToInt32(reader["author_id"]),
                        };

                        books.Add(book);
                    }
                }
            }

            CloseConnection();
            return books;
        }
    }
}