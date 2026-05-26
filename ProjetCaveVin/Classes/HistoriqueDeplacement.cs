 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetCaveVin.Model.Connexion;
//using ProjetCaveVin.Model.Tables;


namespace ProjetCaveVin.Classes
{
    public class HistoriqueDeplacement
    {
        public int Id { get; set; }
        public DateTime DateDeplacement { get; set; }

        public int IdUtilisateur { get; set; }
        public Utilisateur Utilisateur { get; set; }

        public int IdBouteille { get; set; }
        public Bouteille Bouteille { get; set; }

        public int IdEmplacement { get; set; }
        public Emplacement Emplacement { get; set; }


       
    }
}
