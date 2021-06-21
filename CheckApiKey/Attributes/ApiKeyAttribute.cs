using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using static Common.Glb;
using System.Text;
using System.Globalization;
using System.Linq;

// >>>>>>>>>>> SE VOCE PRETENDE USAR WEB ASSEMBLY CONSULTE A DOCUMENTAÇÃO RECOMENDADA ABAIXO <<<<<<<<<<<<<<<<<<<<<<<<
// ==================================================================================================================
// Configuration in a Blazor WebAssembly app is visible to users.
// Don't store app secrets, credentials, or any other sensitive data in the configuration of a Blazor WebAssembly app.
// For more information on configuration providers, see Configuration in ASP.NET Core.

namespace CheckApiKey.Attributes
{
    [AttributeUsage(validOn:AttributeTargets.Class)]
    public class ApiKeyValidationAttribute : Attribute, IAsyncActionFilter
    {       
        public async Task OnActionExecutionAsync( ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //  Seta contador de tempo
            DataI = DateTime.Now;

            // Get configuration file
            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            //  Get configuration parameters value
            var KeyName = config.GetValue<string>("APIs:CheckApi:KeyName");         
            var KeyValue = config.GetValue<string>("APIs:CheckApi:KeyValue");       // Hash do codigo de acesso

            // Transforms parameter hexadecinal string into byte array.
            // Lembra que o codigo gerado mostrava 2 caracteres para cada byte
            // This must be done to compare with user supplied key
            byte[] ArrKeyValue = new byte[KeyValue.Length / 2];
            for (int index = 0; index < ArrKeyValue.Length; index++)
            {
                string byteValue = KeyValue.Substring(index * 2, 2);
                ArrKeyValue[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            
            //  Scans Http request header for user supplied apikey value
            if ( !context.HttpContext.Request.Headers.TryGetValue( KeyName, out var HttpApiKey))
            {
                // No valid value found for this keyname 
                context.Result = new ContentResult() { StatusCode = 401, Content = "Api key invalid ot not supplied" };
                return;
            }

            //  Get Keyvalue supplied by http request header
            SHA256 mySHA256 = SHA256.Create();            
            byte[] ArrHttpApiKey = mySHA256.ComputeHash(Encoding.ASCII.GetBytes( HttpApiKey));           

            //  Compares two byte arrays - to check jey match
            if ( !ArrKeyValue.SequenceEqual( ArrHttpApiKey))
            {                
                context.Result = new ContentResult() { StatusCode = 401, Content = "Unauthorized client" };
                return;
            }

            //  Holds its place in pipeline
            await next();
        }
    }
}