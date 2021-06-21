using System;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Common.Glb;

namespace CheckApiKey.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class BasicAuthenticationAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //  Seta contador de tempo
            DataI = DateTime.Now;

            //  Checks for Authorizatiom Header
            if ( string.IsNullOrEmpty( context.HttpContext.Request.Headers["Authorization"]))
            {
                //  Nao existe o header de autorização
                context.Result = new ContentResult() { StatusCode = 401, Content = "Authorization invalid or not supplied" };
                return ;
            }

            //  Get Authorization parameters
            string authHeader = context.HttpContext.Request.Headers["Authorization"].ToString().Trim();

            // Checks for Authorization = BASIC
            if (!authHeader.Substring(0, "Basic ".Length).Equals("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                // Náo existe o indicador de tipo de autorização
                context.Result = new ContentResult() { StatusCode = 401, Content = "Authorization type invalid or not supplied" };
                return ;
            }

            //  Extract UserName and Password (base64 encoded) from Authorization parameters
            string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();

            var bytes = Convert.FromBase64String(encodedUsernamePassword);
            var UsernamePassword = Encoding.UTF8.GetString(bytes);

            int separatorIndex = UsernamePassword.IndexOf(':');

            string username = UsernamePassword.Substring( 0, separatorIndex);
            string password = UsernamePassword.Substring( separatorIndex + 1);

            // Check for supplied UserName as Password validity
            if ( !UserValidate( context, username, password))
            {
                // No match value found for this keyname 
                context.Result = new ContentResult() { StatusCode = 400, Content = $"Invalid pair >> User Name : {username} / Password : {password}" };
                return ;
            }

            //  Holds its place in pipeline
            await next();
        }

        public bool UserValidate(ActionExecutingContext context, string User, string Pass)
        {
            // Get configuration file
            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            //  Get configuration parameters value
            var KeyName = config.GetValue<string>("BasicAuthentication:UserName");
            var KeyValue = config.GetValue<string>("BasicAuthentication:Password");

            // Here you check User and Pass against
            if ( User.Equals( KeyName) && Pass.Equals( KeyValue))
            {
                // Generic Id
                string[] Roles = { "Contabil", "Comercial" };                
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(KeyName), Roles);
                return true;
            }
            return false;
        }
    }
}