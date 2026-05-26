using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCaveVin.Classes
{
    public class Session
    {
        public static Utilisateur CurrentUser { get; set; }
        public static bool IsConnected => CurrentUser != null;
        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}