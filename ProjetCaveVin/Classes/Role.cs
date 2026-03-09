using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetCaveVin.Model.Connexion;


namespace ProjetCaveVin.Classes
{
    public class Role
    {

        public int id_role { get; set; }
        public string Nom { get; set; }


        public Role() { }

        public Role(int id, string name)
        {
            id_role = id;
            Nom = name;
        }

        
    }
}
