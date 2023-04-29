using Microsoft.Extensions.Configuration;
using Npgsql;
using StackExchange.Redis;

public class TestFixture : IDisposable
{
    public IConfiguration _configuration;
    private string _redisTestConnectionString;
    private string _postgresTestConnectionString;
    private string _postgresDevelopmentConnectionString;

    public TestFixture()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        _postgresDevelopmentConnectionString = _configuration.GetConnectionString("Postgres");
        _postgresTestConnectionString = _configuration.GetConnectionString("TestPostgres");
        _redisTestConnectionString = _configuration.GetConnectionString("TestRedis");

        if (!DatabaseTestUtils.DatabaseExists(_postgresDevelopmentConnectionString, "bookstore_test"))
        {
            DatabaseTestUtils.ExecuteScript(_postgresDevelopmentConnectionString, "../../../Database/create-database.sql");
        }
        
        DatabaseTestUtils.ExecuteScript(_postgresTestConnectionString, "../../../Database/drop-tables.sql");
        DatabaseTestUtils.ExecuteScript(_postgresTestConnectionString, "../../../Database/create-tables.sql");
        DatabaseTestUtils.ExecuteScript(_postgresTestConnectionString, "../../../Database/insert-data.sql");

        ClearRedisDatabase();
    }
    
    public void Dispose()
    {
        // Drop postgre database
        DropTestDatabase();

        // Clear test Redis database
        ClearRedisDatabase();
    }

    public void ClearRedisDatabase()
    {
        var redisTestConnectionString = _configuration.GetConnectionString("Redis");
        var options = ConfigurationOptions.Parse(_redisTestConnectionString + ",allowAdmin=true");
        var redis = ConnectionMultiplexer.Connect(options);
        var server = redis.GetServer(options.EndPoints.First());
        server.FlushDatabase(options.DefaultDatabase.GetValueOrDefault());
    }
    
    public void DropTestDatabase()
    {
        // Connect to the development database
        using (var connection = new NpgsqlConnection(_postgresDevelopmentConnectionString))
        {
            connection.Open();
        
            // Terminate all connections to the "bookstore_test" database
            using (var cmd = new NpgsqlCommand("SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = 'bookstore_test';", connection))
            {
                cmd.ExecuteNonQuery();
            }

            // Drop the "bookstore_test" database
            using (var cmd = new NpgsqlCommand("DROP DATABASE IF EXISTS bookstore_test;", connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }

}
