using EvoTrackBack.Tools;
using Microsoft.Data.SqlClient;
using ProjetCaveVin.Model.Connexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCaveVin.Repositories
{
    public class UtilisateurRepository (ISqlConnectionFactory db)
    {
        public ISqlConnectionFactory _db = db;
        public  List<Utilisateur> GetAllUtilisateur()
        {
            var utilisateurs = new List<Utilisateur>();

            using SqlConnection connection = (SqlConnection)(_db.CreateConnection());
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT u.id_utilisateur, u.Nom, u.Prenom, u.Email, u.PasswordHash, u.Salt, 
                           r.id_role, r.nom
                    FROM Utilisateur u
                    INNER JOIN Role r ON u.id_role_utilisateur = r.id_role";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var utilisateur = new Utilisateur
                        {
                            id_utilisateur = reader.GetInt32(0),
                            Nom = reader.GetString(1),
                            Prenom = reader.GetString(2),
                            Email = reader.GetString(3),
                            PasswordHash = reader.GetString(4),
                            Salt = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Role = new Role
                            {
                                id_role = reader.GetInt32(6),
                                Nom = reader.GetString(7)
                            }
                        };
                        utilisateurs.Add(utilisateur);
                    }
                }
            }

            connection.Close();
            return utilisateurs;
        }

        //  Vérifie les identifiants
        public  Utilisateur GetByCredentials(string email, string password)
        {
            using SqlConnection connection = (SqlConnection)(_db.CreateConnection());
            connection.Open();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                    SELECT id_utilisateur, u.Nom, Prenom, Email, PasswordHash, Salt, r.id_role, r.nom
                    FROM Utilisateur u
                    INNER JOIN Role r ON u.id_role_utilisateur = r.id_role
                    WHERE u.Email = @Email";
                    command.Parameters.AddWithValue("@Email", email);

                    using var reader = command.ExecuteReader();
                    if (!reader.Read())
                        return null;

                    string storedHash = reader.GetString(reader.GetOrdinal("PasswordHash"));
                    string storedSalt = reader.IsDBNull(reader.GetOrdinal("Salt")) ? null : reader.GetString(reader.GetOrdinal("Salt"));

                    bool isValid = false;
                    if (!string.IsNullOrEmpty(storedSalt))
                    {
                        isValid = PasswordHelper.VerifyPassword(password, storedHash, storedSalt);
                    }
                    else
                    {
                        isValid = storedHash == password;
                    }

                    if (!isValid)
                        return null;

                    return new Utilisateur 
                    {
                        id_utilisateur = reader.GetInt32(reader.GetOrdinal("id_utilisateur")),
                        Nom = reader.GetString(reader.GetOrdinal("Nom")),
                        Prenom = reader.GetString(reader.GetOrdinal("Prenom")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        PasswordHash = storedHash,
                        Role = new Role
                        {
                            id_role = reader.GetInt32(reader.GetOrdinal("id_role")),
                            Nom = reader.GetString(reader.GetOrdinal("nom"))
                        }
                    };
                }
            }
            finally
            {
                connection.Close();
            }
        }



        public void InsertUtilisateur(string nom, string prenom, string email, string plainPassword, string nomRole)
        {
            var (hash, salt) = PasswordHelper.HashPassword(plainPassword);
            int roleId;

            using SqlConnection connection = (SqlConnection)(_db.CreateConnection());
            connection.Open();

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT id_role FROM Role WHERE nom = @Role";
                cmd.Parameters.AddWithValue("@Role", nomRole);
                var result = cmd.ExecuteScalar();
                if (result == null)
                    throw new Exception("Rôle invalide.");
                roleId = Convert.ToInt32(result);
            }

            using (var cmdInsert = connection.CreateCommand())
            {
                cmdInsert.CommandText = @"
            INSERT INTO Utilisateur (Nom, Prenom, Email, PasswordHash, Salt, id_role_utilisateur)
            VALUES (@Nom, @Prenom, @Email, @PasswordHash, @Salt, @IdRole)";
                cmdInsert.Parameters.AddWithValue("@Nom", nom);
                cmdInsert.Parameters.AddWithValue("@Prenom", prenom);
                cmdInsert.Parameters.AddWithValue("@Email", email);
                cmdInsert.Parameters.AddWithValue("@PasswordHash", hash);
                cmdInsert.Parameters.AddWithValue("@Salt", salt);
                cmdInsert.Parameters.AddWithValue("@IdRole", roleId);

                cmdInsert.ExecuteNonQuery();
            }

            connection.Close();
        }

        public void DeleteUtilisateur(string email)
        {
            using SqlConnection connection = (SqlConnection)(_db.CreateConnection());
            connection.Open();

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @" DELETE FROM Utilisateur WHERE Email = @Email;";
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.ExecuteNonQuery();

            }
        }
    }
}
