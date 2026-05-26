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
    public class HDRepository (ISqlConnectionFactory db)
    {
        public ISqlConnectionFactory _db = db;
        public void EnregistrerMouvement(int idBouteille, int idNouveauEmplacement, int idUtilisateur)
        {
            using SqlConnection connection = (SqlConnection)(_db.CreateConnection());
            connection.Open();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        INSERT INTO HistoriqueDeplacement (Date_Deplacement,id_utilisateur,id_bouteille, id_emplacement )
                        VALUES (@Date, @IdUser,@IdBouteille, @IdEmplacement )";

                    command.Parameters.AddWithValue("@Date", DateTime.Now);
                    command.Parameters.AddWithValue("@IdUser", idUtilisateur);
                    command.Parameters.AddWithValue("@IdBouteille", idBouteille);
                    command.Parameters.AddWithValue("@IdEmplacement", idNouveauEmplacement);

                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
            }
        }


        public List<HistoriqueDeplacement> GetAllHistoriqueDeplacements()
        {
            var listeRetour = new List<HistoriqueDeplacement>();

            using SqlConnection connection = (SqlConnection)(_db.CreateConnection());
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id_historique, Date_Deplacement, id_utilisateur, id_bouteille, id_emplacement FROM HistoriqueDeplacement ORDER BY Date_Deplacement DESC";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var historique = new HistoriqueDeplacement
                        {
                            Id = reader.GetInt32(0),
                            DateDeplacement = reader.GetDateTime(1),
                            IdUtilisateur = reader.GetInt32(2),
                            IdBouteille = reader.GetInt32(3),
                            IdEmplacement = reader.GetInt32(4)
                        };

                        listeRetour.Add(historique);
                    }
                }
            }
            connection.Close();

            return listeRetour;
        }
    }
}
