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
    public class RoleRepository (ISqlConnectionFactory db)
    {
        public ISqlConnectionFactory _db = db;
        public List<Role> GetAllRole()
        {

            var roles = new List<Role>();
            using SqlConnection connection = (SqlConnection)(_db.CreateConnection());
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id_role, Nom FROM Role";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(new Role
                        {
                            id_role = reader.GetInt32(0),
                            Nom = reader.GetString(1)
                        });

                    }
                }
            }

            connection.Close();
            return roles;
        }

        public Role GetById(int id)
        {
            using SqlConnection connection = (SqlConnection)(_db.CreateConnection());
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id_role, nom FROM Role WHERE id_role = @Id";
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Role
                        {
                            id_role = reader.GetInt32(0),
                            Nom = reader.GetString(1)
                        };
                    }
                }
            }

            connection.Close();
            return null;
        }

        public Role GetByName(string nom)
        {
            using SqlConnection connection = (SqlConnection)(_db.CreateConnection());
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id_role, nom FROM Role WHERE nom = @Nom";
                command.Parameters.AddWithValue("@Nom", nom);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Role
                        {
                            id_role = reader.GetInt32(0),
                            Nom = reader.GetString(1)
                        };
                    }
                }
            }

            connection.Close();
            return null;
        }
    }
}
