using MongoDB.Bson.IO;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Authentication;
using System.Text.RegularExpressions;

namespace ISTA.SecureApp.Api.Middlewares
{
    public class Middleware
    {

        private readonly RequestDelegate _next;

        public Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var query = context.Request.QueryString.Value;

            if (!IsValidInput(query ?? ""))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Forbidden");
                return;
            }

            if (context.Request.Method == HttpMethods.Post)
            {
                // Habilitar el buffer para permitir múltiples lecturas del cuerpo
                context.Request.EnableBuffering();

                if (context.Request.HasFormContentType)
                {
                    var form = await context.Request.ReadFormAsync();
                    foreach (var field in form)
                    {
                        if (!IsValidInput(field.Value))
                        {
                            context.Response.StatusCode = 400;
                            await context.Response.WriteAsync($"Invalid form input detected in field {field.Key}.");
                            return;
                        }
                    }
                }

                context.Request.Body.Position = 0;
            }

            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)GetErrorCode(error);
                await response.WriteAsync(error.Message);
            }
        }

        private bool IsValidInput(string input)
        {
            if (string.IsNullOrEmpty(input)) return true;

            string patternXSS = @"<.*?>";
            if (Regex.IsMatch(input, patternXSS, RegexOptions.IgnoreCase)) return false;

            string patternSQL = @"(\b(ALTER|DROP|INSERT|DELETE|SELECT|UPDATE|CREATE|REPLACE|TRUNCATE|EXEC)\b)";
            if (Regex.IsMatch(input, patternSQL, RegexOptions.IgnoreCase)) return false;

            return true;
        }

        private static HttpStatusCode GetErrorCode(Exception e)
        {
            return e switch
            {
                UnauthorizedAccessException _ => HttpStatusCode.Unauthorized,
                ValidationException _ => HttpStatusCode.BadRequest,
                FormatException _ => HttpStatusCode.BadRequest,
                AuthenticationException _ => HttpStatusCode.Unauthorized,
                NotImplementedException _ => HttpStatusCode.NotImplemented,
                _ => HttpStatusCode.InternalServerError,
            };
        }
    }
}
