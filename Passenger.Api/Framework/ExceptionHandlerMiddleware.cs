using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Passenger.Api.Framework
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next; //wymagalność pola podyktowana jest konwencją rowiązania middlware'owego

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // spróbuj wykonać nasze aktualne żadanie http, a jak dostaniesz błąd...
            }
            catch(Exception exception)
            {
                // przetwarzanie błędu
                await HandleExceptionAsync(context, exception);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorCoder = "error";
            var statusCode = HttpStatusCode.BadRequest;
            var exceptionType = exception.GetType(); 
            
            switch(exception)
            {
                case Exception e when exceptionType == typeof(UnauthorizedAccessException): // featrue z wersji 7 c# -> pattern matching taki case bez stałych warunków
                    statusCode = HttpStatusCode.Unauthorized;
                    break;
                // TODO: custom exception
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }
            // przemapowanie błędu i komuniaktua na format jsona
            var response = new { code = errorCoder, message = exception.Message };
            // tworzenie odpowiedzi na zapytanie http
            var payload = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(payload);
        }
    
    }
}