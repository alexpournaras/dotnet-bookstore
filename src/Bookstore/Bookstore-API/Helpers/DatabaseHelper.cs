using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace BookstoreAPI.Helpers
{
    public class DatabaseHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public DatabaseHelper(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public string GetConnString()
        {
            if (_environment.EnvironmentName == "Test") {
                return _configuration.GetConnectionString("TestPostgres");
            }
            else
            {
                return _configuration.GetConnectionString("Postgres");
            }
        }
    }
}
