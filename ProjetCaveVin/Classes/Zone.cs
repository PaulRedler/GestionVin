using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCaveVin.Classes
{
    public class Zone
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Superficie { get; set; }

        // Relation parent/enfant
        public int? IdZoneParent { get; set; }
        public Zone ZoneParent { get; set; }

        public List<Zone> ZonesEnfants { get; set; } = new List<Zone>();
        public List<Emplacement> Emplacements { get; set; } = new List<Emplacement>();
    }
}
