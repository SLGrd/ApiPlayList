using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace Common
{
    public static class Glb
    {
        public static string ApiKeyCrd;
        public static string HashKey = "To be supplied";
        public static string LastGeneratedToken;

        public static DateTime DataI;
    }
    public static class SimulaDB
    {
        public static UserToken Get(string username, string password)
        {
            var users = new List<UserToken>();
            users.Add(new UserToken { Id = 1, Username = "Gerubal",   Password = "Pascoal",     Role = "Pessoal"    });
            users.Add(new UserToken { Id = 2, Username = "Castor",    Password = "Construtor",  Role = "Engenharia" });
            users.Add(new UserToken { Id = 3, Username = "Vendedor",  Password = "Vendas",      Role = "Marketing"  });
            return users.Where(x => x.Username.ToLower() == username.ToLower() && x.Password == password).FirstOrDefault();
        }
    }
}