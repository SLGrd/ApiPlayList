using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static Common.Glb;

namespace CheckApiKey.Middleware
{
    public class ApiKeyMiddleware
    {        
        private readonly RequestDelegate Next;        
        public ApiKeyMiddleware( RequestDelegate next) { Next = next;}
        public async Task InvokeAsync( HttpContext context)
        {
            DataI = DateTime.Now;            

            ////  Http request header for api key code
            //if (!context.Request.Headers.TryGetValue( ApiKeyId, out var extractedApiKey))
            //{
            //    context.Response.StatusCode = 401;
            //    await context.Response.WriteAsync("Middleware - Api key not  supplied");
            //    return;
            //}

            ////  Checks match of supplied code with Api key signature
            //if (!extractedApiKey.Equals( ApiKeyValue)
            //{
            //    context.Response.StatusCode = 401;
            //    await context.Response.WriteAsync("Middleware - Unauthorized client" );
            //    return;
            //}
            //  Holds its place in pipeline

           _ = Next( context);
            
            await context.Response.WriteAsync($"Tempo de execução - { (DateTime.Now - DataI).Milliseconds }");
        }
    }
}