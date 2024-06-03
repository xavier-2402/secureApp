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

            await _next(context);
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
    }
}
