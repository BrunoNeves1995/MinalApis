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
         [Route("")]
         public class HomeController : ControllerBase
         {
             [HttpGet("")]
             public IActionResult Get()
             {
                 return Ok( new {
                     Status = "Online"
                 });
             }
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
         
    
### VALIDAÇÕES
  
  - ModelState verifica se o nosso modelo EditorCategoryViewModel recebido esta valido, baseado nas anotaçoes [Required] que fica dentro do modelo
    
    - Modelo
    
           public class EditorCategoryViewModel
          {   
              [Required(ErrorMessage = "Nome é obrigatório")]
              public string Name { get; set; } = null!;

              [Required(ErrorMessage = "Slug é obrigatório")]
              public string Slug { get; set; } = null!;
          }
          
    - Codigo

            [HttpPost("v1/categories")]
            public async Task<IActionResult> PostAsync(
                [FromServices] DataContext context,
                [FromBody] EditorCategoryViewModel model
            )
            {   
                if(!ModelState.IsValid)
                    return BadRequest();

                try
                {   
                    var category = new Category{
                        Id = 0,
                        Name = model.Name.Replace("-", ""),
                        Slug = model.Slug.ToLower(),
                    };

                    await context.Categories!.AddAsync(category);
                    await context.SaveChangesAsync();

                    return Created($"v1/categories/{category.Id}", category);
                }
                catch (DbUpdateException)
                {
                    return StatusCode(500, "01XE3 - Não foi possivel incluir uma nova categoria");
                }
                catch (Exception)
                {
                    return StatusCode(500, "01XE4 - Falha interna no servidor");
                }
            }



### PADRONIZANDO OS RETORNO DA NOSSA API

   - Devemos criar uma nova classe ResultViewModel onde todo a requição vai ser retornado o nosso ResultViewModel
     
     - Quando der erro vai ser retornado o nosso modelo ResultViewModel com o erro dentro
     - quando for BadRequest vai ser retornado o nosso modelo ResultViewModel com a lista erro dentro
     - Qaundo for sucesso vai ser retornado o nosso modelo ResultViewModel com os dados de sucesso da nossa requisição
       
            public class ResultViewModel<T>
            {   
                // sucesso e erros
                public ResultViewModel(T data, List<string> errors){
                    Data = data;
                    Errors = errors;
                }

                // sucesso
                public ResultViewModel(T data)
                {
                    Data = data;
                }

                // lista error
                public ResultViewModel(List<string> errors)
                {
                    Errors = errors;
                }

                // quando tenho somee um erro, adiciono o erro na lista de erros
                public ResultViewModel(string erro)
                {
                    Errors.Add(erro);
                }

                public T? Data { get; private set; }
                public List<string>? Errors { get; private set; } = new ();
            }
            
     #### Padronizando o nosso retorno para o CategoryController
     
       - Quando tipo retornamos for uma lista de categorias
     
             return Ok(new ResultViewModel<List<Category>>(data: categories));
             
       - Qauando o retorno for uma string     
            
             catch (Exception)
             {
                 return StatusCode(500, new new ResultViewModel<Category>(erro: "01XE1 - Falha interna no servidor"));
             }
        
       - Quando tipo retornamos for uma categoria
       
                 return Ok(new ResultViewModel<Category>(data: categorie));
                 
       #### Utilizando metodos de extensão
   
       - Quando tipo retornamos for uma lista de objetos, iremos converter [ModelState.Values] que é um ValueEnumerable para string
         - Criaremos um metodo de extensão, que permite adicionar novas funcionalidade dentro das outras classes

               return BadRequest(new ResultViewModel<Category>(errors: ModelState.GetErrors()));
                 
### CONFIGURANDO O COMPORTAMENTO DA API
  
   - Desabilitando a validação automatica do ModelState do Asp Net
   
          builder.Services.AddControllers()
         .ConfigureApiBehaviorOptions(options => {
             options.SuppressModelStateInvalidFilter = true;
         });


   - Com isso, somos obrigado a informa que queremos a validção do ModelState
   
             if(!ModelState.IsValid)
                return BadRequest();



### AUTENTICAÇÃO E AUTORIZAÇÃO
                
   
