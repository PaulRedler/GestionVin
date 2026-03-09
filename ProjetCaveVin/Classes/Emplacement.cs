using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCaveVin.Model.Classes
{
    public class Emplacement
    {

        public int Id { get; set; }
        public string Code_Emplacement { get; set; }

        public int Limite_Bouteille { get; set; }
        public int Qte_Bouteille { get; set; }
        public int id_zone { get; set; }

    }
}
