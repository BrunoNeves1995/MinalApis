using System.IO.Compression;
using System.Collections.Immutable;
using System.Text;
using System.Text.Json.Serialization;
using Blog;
using Blog.Data;
using Blog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


ConfigurandoAutenticaoJWT(builder);
HabilitandoMvcNaAplicacao(builder);
ConfiguracaoService(builder);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Urls.Add("https://localhost:3000");

MapeandoController(app);



app.Run();

void MapeandoController(WebApplication app)
{

    Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey")!;
    Configuration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName")!;
    Configuration.ApiKey = app.Configuration.GetValue<string>("ApiKey")!;

    var smtp = new Configuration.SmtpConfiguration();
    app.Configuration.GetSection("Smtp").Bind(smtp);
    Configuration.Smtp = smtp;

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.UseStaticFiles();
    app.UseResponseCompression();

    if(app.Environment.IsDevelopment())
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Projeto rodando em ambiente de desenvolvimento");
            Console.ResetColor();

            app.UseSwagger();
            app.UseSwaggerUI();
        }

}

void HabilitandoMvcNaAplicacao(WebApplicationBuilder builder)
{
    builder.Services.AddMemoryCache();

    builder.Services.AddResponseCompression(x =>
    {
        x.Providers.Add<GzipCompressionProvider>();
    });

    builder.Services.Configure<GzipCompressionProviderOptions>(x =>
        {
            x.Level = CompressionLevel.Optimal;
        });

    builder
        .Services
        .AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        })
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        // x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
    })
    ;
}

void ConfigurandoAutenticaoJWT(WebApplicationBuilder builder)
{
    var chave = Encoding.ASCII.GetBytes(Configuration.JwtKey);
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(chave),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
}

void ConfiguracaoService(WebApplicationBuilder builder)
{   
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
    
    builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
    builder.Services.AddTransient<TokenService>();
    builder.Services.AddTransient<EmailService>();
}

