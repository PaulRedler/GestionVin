using System;

namespace ProjetCaveVin.Model.Connexion
{
    public class Connexion
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }          
        public string Database { get; set; }      
        public string Instance { get; set; }      
        public int Port { get; set; } = 1433;    

        public Connexion() { }

        public Connexion(string username, string password, string host, string database, string instance = "SQLEXPRESS", int port = 1433) // changement du SQLEXPRESS02
        {
            Username = username;
            Password = password;
            Host = host;
            Database = database;
            Instance = instance;
            Port = port;
        }

        public override string ToString()
        {
            // Chaîne complète de connexion SQL Server
            string serverPart = string.IsNullOrEmpty(Instance) ? Host : $"{Host}\\{Instance}";
            if (Port != 1433)
                serverPart += $",{Port}";

            return $"Server={serverPart};Database={Database};User Id={Username};Password={Password};Encrypt=False;";
        }
    }
}
