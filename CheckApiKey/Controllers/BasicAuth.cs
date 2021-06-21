using CheckApiKey.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using static Common.Glb;

namespace CheckApiKey.Controllers
{
    [ApiController]
    [Route("BasicAuth")]
    [BasicAuthentication]    
    public class BasicAuth : Controller
    {
        [HttpGet("Ping")]       
        // Este endereço é URL + Controller + Ping =>  https://Localhost:44034/BasicAuth/Ping
        public string GetAllAsync() => $" {DateTime.Now:hh:mm:ss}" + $" : Basic Auth OK - Tempo de resposta { (DateTime.Now - DataI).Milliseconds } ms ";

        [HttpGet("Pong")]               
        // Este endereço é URL + Controller + Ping =>  https://Localhost:44034/BasicAuth/Pong
        public string Access() => $" {DateTime.Now:hh:mm:ss}" + $" : Pong Auth OK - Tempo de resposta { (DateTime.Now - DataI).Milliseconds } ms ";
    }
}