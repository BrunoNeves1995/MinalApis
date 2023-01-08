using introducao.Data;

var builder = WebApplication.CreateBuilder(args);
HabilitandoMvcNaAplicacao(builder);


var app = builder.Build();

MapeandoOController(app);
app.Run();


void HabilitandoMvcNaAplicacao(WebApplicationBuilder builder)   
{
    builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => {
        options.SuppressModelStateInvalidFilter = true;
    });
    builder.Services.AddDbContext<DataContext>();
}

void MapeandoOController(WebApplication app)
{
    app.MapControllers();
}