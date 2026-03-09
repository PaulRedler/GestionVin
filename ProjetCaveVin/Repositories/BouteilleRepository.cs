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
    public class BouteilleRepository(ISqlConnectionFactory db)
    {
        private readonly ISqlConnectionFactory _db = db;

        public List<Bouteille> GetAllBouteilles()
        {
            List<Bouteille> bouteilles = new();
            using SqlConnection connection = (SqlConnection)(_db.CreateConnection());
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    WITH DernierDeplacement AS (
                        SELECT 
                            h.id_bouteille,
                            h.id_emplacement,
                            h.Date_Deplacement,
                            ROW_NUMBER() OVER (PARTITION BY h.id_bouteille ORDER BY h.Date_Deplacement DESC) AS rn
                        FROM HistoriqueDeplacement h
                    )
                    SELECT 
                        b.id_bouteille,
                        b.Libelle,
                        b.Millesime,
                        b.Prix,
                        t.LibelleType AS Type,
                        z.Code AS ZoneCode,
                        e.Code_Emplacement,
                        t.PhotoURL
                    FROM Bouteille b
                    INNER JOIN DernierDeplacement d 
                        ON b.id_bouteille = d.id_bouteille AND d.rn = 1
                    INNER JOIN Emplacement e 
                        ON d.id_emplacement = e.id_emplacement
                    INNER JOIN Zone z 
                        ON e.id_zone = z.id_zone
                    INNER JOIN TypeBouteille t
                        ON b.id_type = t.id_type;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bouteilles.Add(new Bouteille
                        {
                            Id = reader.GetInt32(0),
                            Libelle = reader.GetString(1),
                            Millesime = reader.GetString(2),
                            Prix = reader.GetDecimal(3),
                            Type = reader.GetString(4),
                            Code = reader.GetString(5),
                            Code_Emplacement = reader.GetString(6),
                            Photo = reader.GetString(7),
                        });
                    }
                }
            }

            return bouteilles;
        }
        public void AjouterBouteille(Bouteille nouvelleBouteille, int idEmplacement, int idUtilisateur)
        {
            using SqlConnection connection = (SqlConnection)(_db.CreateConnection());
            connection.Open();

            using (connection)
            {
                var transaction = connection.BeginTransaction();

                try
                {
                    var cmdBouteille = connection.CreateCommand();
                    cmdBouteille.Transaction = transaction;
                    cmdBouteille.CommandText = @"
                INSERT INTO Bouteille (Libelle, Millesime, Contenance, Prix, Code, Type, Photo, id_origine)
                VALUES (@Lib, @Mil, @Cont, @Prix, @Code, @Type, @Photo, @Origine);
                SELECT SCOPE_IDENTITY();";

                    cmdBouteille.Parameters.AddWithValue("@Lib", nouvelleBouteille.Libelle);
                    cmdBouteille.Parameters.AddWithValue("@Mil", nouvelleBouteille.Millesime ?? (object)DBNull.Value);
                    cmdBouteille.Parameters.AddWithValue("@Cont", nouvelleBouteille.Contenance);
                    cmdBouteille.Parameters.AddWithValue("@Prix", nouvelleBouteille.Prix);
                    cmdBouteille.Parameters.AddWithValue("@Code", nouvelleBouteille.Code ?? "NO_CODE");
                    cmdBouteille.Parameters.AddWithValue("@Type", nouvelleBouteille.Type); // Attention, ici c'est peut-être id_type selon ta base
                    cmdBouteille.Parameters.AddWithValue("@Photo", nouvelleBouteille.Photo ?? (object)DBNull.Value);
                    cmdBouteille.Parameters.AddWithValue("@Origine", nouvelleBouteille.IdOrigine);

                    int newIdBouteille = Convert.ToInt32(cmdBouteille.ExecuteScalar());

                    //Insérer l'Historique - sinon aucun emplacement
                    var cmdHist = connection.CreateCommand();
                    cmdHist.Transaction = transaction;
                    cmdHist.CommandText = @"
                INSERT INTO HistoriqueDeplacement (Date_Deplacement, id_utilisateur, id_bouteille, id_emplacement)
                VALUES (GETDATE(), @User, @Bouteille, @Emplacement)";

                    cmdHist.Parameters.AddWithValue("@User", idUtilisateur);
                    cmdHist.Parameters.AddWithValue("@Bouteille", newIdBouteille);
                    cmdHist.Parameters.AddWithValue("@Emplacement", idEmplacement);

                    cmdHist.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    // si erreur , on annule tout meme la creation de la bouteille
                    transaction.Rollback();
                    throw;
                }
            }
            connection.Close();
        }
        public List<Bouteille> GetAllBouteillesAvecEmplacement()
        {
            var list = new List<Bouteille>();
            using SqlConnection connection = (SqlConnection)(_db.CreateConnection());
            connection.Open();

            using (var cmd = connection.CreateCommand())
            {

                cmd.CommandText = @"
            use cave;
SELECT 
                b.id_bouteille, 
                b.Libelle, 
                b.Millesime, 
                b.Contenance, 
                b.Prix, 
                b.id_origine,
                e.Code_Emplacement,
                z.Code
            FROM Bouteille b
            INNER JOIN HistoriqueDeplacement h ON b.id_bouteille = h.id_bouteille
            INNER JOIN Emplacement e ON h.id_emplacement = e.id_emplacement
            inner join Zone z on e.id_zone = z.id_zone

            -- FILTRE MAGIQUE : On ne garde que la ligne la plus récente pour chaque bouteille

            WHERE h.Date_Deplacement = (
                SELECT MAX(h2.Date_Deplacement) 
                FROM HistoriqueDeplacement h2 
                WHERE h2.id_bouteille = b.id_bouteille)";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Bouteille
                        {
                            Id = reader.GetInt32(0),
                            Libelle = reader.GetString(1),
                            Millesime = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            Contenance = reader.GetDecimal(3),
                            Prix = reader.GetDecimal(4),
                            IdOrigine = reader.GetInt32(5),
                            Code_Emplacement = reader.GetString(6),
                            Zone = reader.GetString(7)
                        });
                    }
                }
            }
            connection.Close();
            return list;
        }
        public List<Bouteille> GetBouteillesParZone(string codeZone)
        {
            List<Bouteille> bouteilles = new();
            using SqlConnection connection = (SqlConnection)(_db.CreateConnection());
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    WITH DernierDeplacement AS (
                        SELECT 
                            h.id_bouteille,
                            h.id_emplacement,
                            h.Date_Deplacement,
                            ROW_NUMBER() OVER (PARTITION BY h.id_bouteille ORDER BY h.Date_Deplacement DESC) AS rn
                        FROM HistoriqueDeplacement h
                    )
                    SELECT 
                        b.id_bouteille,
                        b.Libelle,
                        b.Millesime,
                        b.Prix,
                        t.LibelleType AS Type,
                        z.Code AS ZoneCode,
                        e.Code_Emplacement,
                        t.PhotoURL
                    FROM Bouteille b
                    INNER JOIN DernierDeplacement d 
                        ON b.id_bouteille = d.id_bouteille AND d.rn = 1
                    INNER JOIN Emplacement e 
                        ON d.id_emplacement = e.id_emplacement
                    INNER JOIN Zone z 
                        ON e.id_zone = z.id_zone
                    INNER JOIN TypeBouteille t
                        ON b.id_type = t.id_type
                    WHERE z.Code = @zone;";

                command.Parameters.AddWithValue("@zone", codeZone);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bouteilles.Add(new Bouteille
                        {
                            Id = reader.GetInt32(0),
                            Libelle = reader.GetString(1),
                            Millesime = reader.GetString(2),
                            Prix = reader.GetDecimal(3),
                            Type = reader.GetString(4),
                            Code = reader.GetString(5),
                            Code_Emplacement = reader.GetString(6),
                            Photo = reader.GetString(7),
                        });
                    }
                }
            }

            return bouteilles;
        }
    }
}
