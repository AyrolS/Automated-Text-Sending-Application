/*connection to database*/

using System;
using MySql.Data.MySqlClient;

namespace SqlConnect
{
    public class DbConnector
    {
        private string connectionString;
        private MySqlConnection? connection;

        public MySqlConnection Connection => connection;

        public DbConnector()
        {
            string server = "server name";
            string database = "database name";
            string user = "xxx";
            string password = "xxx";
            connectionString = $"server={server};database={database};uid={user};password={password}";
            connection  = new MySqlConnection(connectionString);
        }
        public void Open()
        {
            try
            {
                if (connection == null)
                {
                    connection = new MySqlConnection(connectionString);
                }
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to the database: " + ex.Message);
            }
        }

        public void Close()
        {
            try
            {
                connection?.Close();
                Console.WriteLine("Database connection closed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error closing database connection: " + ex.Message);
            }
        }

        public MySqlDataReader? ExecuteQuery(string sqlQuery)
        {
            try
            {
                MySqlCommand command = new MySqlCommand(sqlQuery, connection);
                MySqlDataReader reader = command.ExecuteReader();
                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing query: " + ex.Message);
                return null;
            }
        }

    }
}
