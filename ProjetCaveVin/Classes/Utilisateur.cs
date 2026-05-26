using ProjetCaveVin.Model.Connexion;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace ProjetCaveVin.Classes
{
    public class Utilisateur
    {
        public int id_utilisateur { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }   
        public Role Role { get; set; }

        public Utilisateur() { }

        public Utilisateur(int id, string nom, string prenom, string email, string passwordHash, string salt, Role role)
        {
            id_utilisateur = id;
            Nom = nom;
            Prenom = prenom;
            Email = email;
            PasswordHash = passwordHash;
            Salt = salt;
            Role = role;
        }

        //  Récupère tous les utilisateurs
       



        public override string ToString()
        {
            return $"{Nom} {Prenom} ({Email}) - Rôle: {Role.Nom}";
        }
    }
}
