using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCaveVin.Classes
{
    public class Origine
    {
        public int Id { get; set; }
        public string Ville { get; set; }

        public List<Bouteille> Bouteilles { get; set; } = new List<Bouteille>();
    }
}
