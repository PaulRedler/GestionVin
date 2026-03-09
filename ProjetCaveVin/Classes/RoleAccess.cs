using System;
using System.Collections.Generic;
using ProjetCaveVin.Model.Connexion;
using ProjetCaveVin.Helpers;

namespace ProjetCaveVin.Classes
{
    public class RoleAccess
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

       

    }
}
