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

            string query = "SELECT * FROM library.authors ORDER BY id";

            using (var cmd = new NpgsqlCommand(query, _dbInstance))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Author author = new Author();
                        
                        author.Id = Convert.ToInt32(reader["id"]);
                        author.FirstName = Convert.ToString(reader["first_name"]);
                        author.LastName = Convert.ToString(reader["last_name"]);
                        author.Country = Convert.ToString(reader["country"]);
                        author.Books = Convert.ToInt32(reader["books"]);

                        authors.Add(author);
                    }
                }
            }

            return authors;
        }
    }
}