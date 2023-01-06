# Minal Api
 Curso de web api
 
 ### MAPEANDO UMA REQUISIÇÃO
 
  - Results, metodos que nos da acesso a varios metodos do proprio ASP .NET
    
         app.MapGet("/", () => Results.Ok());
         
   ###### MAPEANDO UMA REQUISIÇÃO E RECEBENDO UM PARAMETRO PELA URL

          app.MapGet("/ {nome}", (string nome) => Results.Ok($"Olá {nome}"));
          
   ###### MAPEANDO UMA REQUISIÇÃO E RECEBENDO UM PARAMETRO NO CORPO DA REQUISIÇÃO
   
   - JSON -> User : Precisamos fazer uma serealização que converte json para um objeto User
   - User -> JSON : Precisamos fazer uma deserealização que converte objeto User para um json

     - Esses 2 tipos de conversão ja é feito automatico pelo ASP .NET
             
           app.MapPost("/", (User user) => Results.Ok($"Olá {user}"));

### CONFIGURANDO O HOMECONTROLLER

  - Essa configuração informa que a nossa API ira retornar apenas JSON
   
   
        [ApiController]
        public class HomeController : ControllerBase
        {

        }

   ###### OS METODOS DENTRO DO CONTROLER, SÃO CHAMADOS DE ACTION
   
     - O atributo [HttpGet] informa que o nossa metodo é do tipo GET
     - O atributo [Route("/")] informa o caminho da nossa rota
     
   
        public String Get()
        {
            return $"Hello Word";
        }
        
        
  ### CONFIGURANDO O CONTROLLER NO ASP NET
  
   - Adicionando os controllers
      
          builder.Services.AddControllers();
    
  - Mapeando os controllers
     
         app.MapControllers();
         
  - Deixa nosso DataContext disponivel como um serviço
  - Com isso o ASP NET vai gerenciar a criação e destruição desse objeto
     
         builder.Services.AddDbContext<DataContext>();
    
    
