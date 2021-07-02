using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Common;
using Common.Models;
using static Services.TokenServices;
using System.Linq;

namespace CheckApiKey.Controllers
{
    [Route("JwtBearer")]
    public class JwtBearer : Controller
    {
        [HttpPost("Login")]
        [AllowAnonymous]
        // Este endereço é URL + Controller + Action =>  https://Localhost:44034/JwtBearer/Login
        public async Task<ActionResult<dynamic>> Login([FromBody] UserToken userTk)
        {
            // Recupera o usuário
            var user = SimulaDB.Get(userTk.Username, userTk.Password);

            // Verifica se o usuário existe
            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            // Gera o Token
            var token = TokenGenerate(user);
            // Oculta a senha
            user.Password = "";
            // Retorna os dados
            return new { token };
        }

        [HttpGet("AcessoLivre")]
        [AllowAnonymous]
        public string Anonimo() 
        {
            // Acesso Livre a qquer usuario
            return $" {DateTime.Now:hh:mm:ss} : Acesso Livre a qquer Usuario. ";
        }

        [HttpGet("Autenticado")]
        [Authorize]
        public string Autenticado() 
        {
            // Se vc quer apenas os roles
            var roles = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            string ClaimsList = string.Empty;
            foreach (Claim c in User.Claims) { 
                if ( c.Type == ClaimTypes.Role)    
                    ClaimsList += $"Role = {c.Value} \r\n"; }

            return $"{DateTime.Now:hh:mm:ss} : Acesso a qquer Usuario Autenticado \r\n" +
                   $"Nome : {User.Identity.Name}, \r\n" + 
                   $"Autenticado : { User.Identity.IsAuthenticated}, \r\n" +
                     ClaimsList + "\r";                   
        }

        [HttpGet("Engenharia")]
        [Authorize(Roles = "Engenharia")]
        public string Engenharia()
        {
            string ClaimsList = string.Empty;
            foreach (Claim c in User.Claims) { ClaimsList += $"{c.GetType().Name} = {c.Value} \r\n"; }

            return $"{DateTime.Now:hh:mm:ss} : Acesso a qquer Usuario Autenticado/Engenharia \r\n" +
                   $"Nome : {User.Identity.Name}, \r\n" +
                   $"Autenticado : { User.Identity.IsAuthenticated}, \r\n" +
                     ClaimsList;
        }

        [HttpGet("Pessoal")]
        [Authorize(Roles = "Pessoal")]
        public string Pessoal()
        {
            string ClaimsList = string.Empty;
            foreach ( Claim c in User.Claims) { ClaimsList += $"{c.GetType().FullName} = {c.Value} \r\n"; }

            return $"{DateTime.Now:hh:mm:ss} : Acesso a qquer Usuario Autenticado/Pessoal \r\n" +
                   $"Nome : {User.Identity.Name}, \r\n" +
                   $"Autenticado : { User.Identity.IsAuthenticated}, \r\n" +
                     ClaimsList;            
        }
    }
}