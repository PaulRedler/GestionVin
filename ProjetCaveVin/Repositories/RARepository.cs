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
    public class RARepository (ISqlConnectionFactory db)
    {
        public ISqlConnectionFactory _db = db;
        public RoleAccess GetByRole(string roleName)
        {
            using SqlConnection connection = (SqlConnection)(_db.CreateConnection());
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id_roleaccess, role_name, password_hash, salt FROM RoleAccess WHERE role_name = @r";
            cmd.Parameters.AddWithValue("@r", roleName);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new RoleAccess
                {
                    Id = Convert.ToInt32(reader["id_roleaccess"]),
                    RoleName = reader.GetString(reader.GetOrdinal("role_name")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                    Salt = reader.GetString(reader.GetOrdinal("salt"))
                };
            }

            return null;
        }

        public void CreateOrInsertRole(string roleName, string plainPassword)
        {
            var (hash, salt) = PasswordHelper.HashPassword(plainPassword);

            using SqlConnection connection = (SqlConnection)(_db.CreateConnection());
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO RoleAccess (role_name, password_hash, salt) VALUES (@r, @h, @s)";
            cmd.Parameters.AddWithValue("@r", roleName);
            cmd.Parameters.AddWithValue("@h", hash);
            cmd.Parameters.AddWithValue("@s", salt);

            cmd.ExecuteNonQuery();
        }

        public bool CheckPassword(string roleName, string password)
        {
            using SqlConnection connection = (SqlConnection)(_db.CreateConnection());
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT password_hash, salt FROM RoleAccess WHERE role_name = @r";
            cmd.Parameters.AddWithValue("@r", roleName);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return false;

            string hash = reader.GetString(reader.GetOrdinal("password_hash"));
            string salt = reader.GetString(reader.GetOrdinal("salt"));

            return PasswordHelper.VerifyPassword(password, hash, salt);
        }

        public List<RoleAccess> GetAllRoles()
        {
            var roles = new List<RoleAccess>();

            using SqlConnection connection = (SqlConnection)(_db.CreateConnection());
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id_roleaccess, role_name, password_hash, salt FROM RoleAccess";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                roles.Add(new RoleAccess
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id_roleaccess")),
                    RoleName = reader.GetString(reader.GetOrdinal("role_name")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                    Salt = reader.GetString(reader.GetOrdinal("salt"))
                });
            }

            return roles;
        }

        public void InsertRoleAccess(string roleName, string password)
        {
            using SqlConnection connection = (SqlConnection)(_db.CreateConnection());
            connection.Open();
            try
            {
                var (hash, salt) = PasswordHelper.HashPassword(password);
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO RoleAccess (role_name, password_hash, salt) VALUES (@r, @h, @s)";
                    cmd.Parameters.AddWithValue("@r", roleName);
                    cmd.Parameters.AddWithValue("@h", hash);
                    cmd.Parameters.AddWithValue("@s", salt);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
