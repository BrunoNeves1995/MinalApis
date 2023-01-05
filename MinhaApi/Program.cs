var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/ {nome}", (string nome) => Results.Ok($"Olá {nome}"));

app.MapPost("/", (User user) => Results.Ok($"Olá {user}"));

app.Run();


public class User
{
    public int Id { get; set; }
    public string? Nome { get; set; }
}