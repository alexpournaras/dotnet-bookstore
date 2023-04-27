using Microsoft.Extensions.Configuration;

namespace BookstoreAPI.Helpers
{
    public class DatabaseHelper
    {
        private readonly IConfiguration _configuration;

        public DatabaseHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnString()
        {
            return _configuration.GetConnectionString("Postgres");
        }
    }
}
