using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace AspResultApiApp
{
    public class HtmlResult : IResult
    {
        string htmlCode;
        public HtmlResult(string htmlCode) => this.htmlCode = htmlCode;
        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.ContentType = "text/html; charset=utf-8";
            await context.Response.WriteAsync(htmlCode);
        }
    }
    public static class ResultsHtmlExtension
    {
        public static IResult Html(this IResultExtensions ext, string htmlCode)
            => new HtmlResult(htmlCode);
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", () => "Home page");
            app.MapGet("/about", 
                () => Results.Text("Страница о нас", 
                "text/plain", 
                System.Text.Encoding.UTF8));

            app.MapGet("/json",
                () => Results.Json(new { Name = "Боб", Age = 23 },
                new(System.Text.Json.JsonSerializerDefaults.General)
                //new()
                //{
                //    PropertyNameCaseInsensitive = true,
                //    NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.WriteAsString
                //}
                ));

            app.MapGet("/old", () => Results.LocalRedirect("/new"));
            app.MapGet("/search", () => Results.Redirect("https://yandex.ru"));

            
            string html = @"<!DOCTYPE html>
                            <html>
                            <head>
                                <meta charset='utf-8' />
                                <title>Hello page</title>
                            </head>
                            <body>
                                <h1>Hello Page</h1>
                            </body>
                            </html>";

            app.MapGet("/hello", () => Results.Extensions.Html(html));


            app.Run();
        }
    }
}