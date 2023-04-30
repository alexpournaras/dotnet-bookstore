using Npgsql;

public class DatabaseTestUtils
{
    public static void ExecuteScript(string connectionString, string scriptPath)
    {
        string script = File.ReadAllText(scriptPath);

        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var cmd = new NpgsqlCommand(script, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
    
    public static bool DatabaseExists(string connectionString, string dbName)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var cmd = new NpgsqlCommand($"SELECT 1 FROM pg_database WHERE datname = '{dbName}';", connection))
            {
                var result = cmd.ExecuteScalar();
                return result != null;
            }
        }
    }
}
