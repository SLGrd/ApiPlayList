using System;
using Microsoft.AspNetCore.Mvc;
using CheckApiKey.Attributes;
using static Common.Glb;

namespace CheckApiKey.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiKeyValidationAttribute]
    public class CheckApiKey : Controller
    {                       
        [HttpGet("Ping")]
        // Este endereço é URL + Controller + Ping =>  https://Localhost:44034/CheckApiKey/Ping
        public string GetAllAsync()    
        {                        
            return $" {DateTime.Now:hh:mm:ss:FF}" +  $" : ok - Tempo de resposta { (DateTime.Now - DataI).Milliseconds } ms ";
        }
    }
}