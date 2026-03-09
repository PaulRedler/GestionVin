using System;
using System.Configuration;
using System.Data;
using System.Runtime.Intrinsics.Arm;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace ProjetCaveVin.Model.Connexion
{
    public class DatabaseConnexion 
    {
        private readonly SqlConnection _connection;
        public SqlConnection Connection => _connection; // accès direct

        public DatabaseConnexion(Connexion connexion)
        {
            _connection = new SqlConnection(connexion.ToString());
        }

        public DatabaseConnexion(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public void Open()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }

        public bool IsOpenConnected()
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problème de connexion : " + ex.Message);
                return false;
            }
        }
        public void Close()
        {
            if (_connection.State != ConnectionState.Closed)
                _connection.Close();
        }

        public int Execute(string query)
        {
            using (var command = new SqlCommand(query, _connection))
            {
                return command.ExecuteNonQuery();
            }
        }

        public SqlDataReader ExecuteReader(string query)
        {
            var command = new SqlCommand(query, _connection);
            return command.ExecuteReader();
        }

        public SqlCommand CreateCommand()
        {
            return _connection.CreateCommand();
        }

        public string GetConnectionString()
        {
            return _connection.ConnectionString;
        }

        public static DatabaseConnexion GetConnection()
        {
            string connectionString = @"Server=localhost\SQLEXPRESS03;Database=cave;Trusted_Connection=True;TrustServerCertificate = True;";

            var db = new DatabaseConnexion(connectionString);
            db.Open();
            return db;
        }

        public static string ConnexionDatabase()
        {
            string connectionString = @"Server=localhost\SQLEXPRESS03;Database=cave;Trusted_Connection=True;TrustServerCertificate = True;";

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("La variable d'environnement 'DB_CONNECTION' n'est pas définie.");
            }

            return connectionString;
        }

    }
}
