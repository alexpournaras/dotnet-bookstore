using Npgsql;
using BookstoreAPI.Helpers;

namespace BookstoreAPI.Repositories
{
    public class Repository
    {
        private readonly NpgsqlConnection _dbInstance;

        public Repository(DatabaseHelper databaseHelper)
        {
            _dbInstance = new NpgsqlConnection(databaseHelper.GetConnString());
        }
        
        protected NpgsqlConnection GetConnection()
        {
            return _dbInstance;
        }

        protected void OpenConnection()
        {
            _dbInstance.Open();
        }

        protected void CloseConnection()
        {
            _dbInstance.Close();
        }
    }
}
