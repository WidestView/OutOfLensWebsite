using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;

namespace OutOfLensWebsite.Models
{
    public class DatabaseConnection : IDisposable
    {
        private readonly MySqlConnection _connection;

        public DatabaseConnection()
        {
            const string server = "localhost";
            const string database = "DB_OOL";
            const string username = "figurantpp";
            const string password = "beep";


            string connectionString = $"SERVER={server};DATABASE={database};UID={username};PASSWORD={password};";

            _connection = new MySqlConnection(connectionString);

            _connection.Open();
        }

        public object Run(string query)
        {
            using var command = new MySqlCommand(query, _connection);
            return command.ExecuteScalar();
        }

        public object Run(string query, Dictionary<string, object> parameters)
        {
            using var command = new MySqlCommand(query, _connection);

            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            object result = command.ExecuteScalar();
            command.Dispose();

            return result;
        }

        public List<Dictionary<string, object>> Query(string query)
        {
            var result = new List<Dictionary<string, object>>();

            using var command = new MySqlCommand(query, _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(Enumerable.Range(0, reader.FieldCount)
                    .ToDictionary(reader.GetName, reader.GetValue));
            }

            return result;
        }

        public List<IEnumerable<object>> LinearQuery(string query)
        {
            var result = new List<IEnumerable<object>>();
            
            using var command = new MySqlCommand(query, _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(Enumerable.Range(0, reader.FieldCount).Select(reader.GetValue));
            }
            return result;
        }

        public List<Dictionary<string, object>> Query(string query, Dictionary<string, object> parameters)
        {
            var result = new List<Dictionary<string, object>>();

            using var command = new MySqlCommand(query, _connection);

            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }
            
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(Enumerable.Range(0, reader.FieldCount)
                    .ToDictionary(reader.GetName, reader.GetValue));
            }

            return result;
        }

        public static void Dump(IEnumerable<Dictionary<string, object>> source)
        {
            foreach (var row in source)
            {
                foreach (var col in row)
                {
                    Console.WriteLine($"[{col.Key}] => {col.Value}");
                }

                Console.WriteLine();
            }
        }

        public DataTable DataTableQuery(string commandString)
        {
            MySqlCommand command = new MySqlCommand(commandString, _connection);

            var reader = command.ExecuteReader();

            //

            DataTable table = new DataTable();

            if (reader.HasRows)
            {
                table.Load(reader);
            }

            //

            reader.Close();

            command.Dispose();

            return table;
        }

        public uint GetLastInsertionId()
        {
            return Convert.ToUInt32(Query("select last_insert_id() as 'ID'").First()["ID"]);
        }

        ~DatabaseConnection()
        {
            Dispose();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}