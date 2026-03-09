using System.Collections.Generic;

namespace ProjetCaveVin.Helpers
{
    public static class RoleAccessHelper
    {
        // Mots de passe génériques pour chaque rôle
        private static readonly Dictionary<string, string> RolePasswords = new Dictionary<string, string>
        {
            { "Serveur", "serveur123" },
            { "Sommelier", "sommelier123" },
            { "Administrateur", "admin123" }
        };

        // Vérifie si le mot de passe saisi correspond au rôle choisi
        public static bool CheckRolePassword(string role, string password)
        {
            if (RolePasswords.ContainsKey(role))
            {
                return RolePasswords[role] == password;
            }
            return false;
        }
    }
}
