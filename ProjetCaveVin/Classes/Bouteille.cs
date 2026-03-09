using ProjetCaveVin.Model.Connexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCaveVin.Model.Classes
{
    public class Bouteille
    {
        public int Id { get; set; }
        public string Libelle { get; set; } = string.Empty;
        public string Millesime { get; set; } = string.Empty;
        public decimal Contenance { get; set; }
        public decimal Prix { get; set; }
        public string Zone { get; set; } = string.Empty;
        //------------------------------------------
        public string Code { get; set; } = string.Empty;
        public string Code_Emplacement { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Photo { get; set; }= string.Empty;

        //------------------------------------------
        public int IdOrigine { get; set; }
        public Origine Origine { get; set; } = new();

        //public TypeBouteille Type { get; set; }
      
        public List<HistoriqueDeplacement> HistoriqueDeplacements { get; set; } = new List<HistoriqueDeplacement>();

    }
}
