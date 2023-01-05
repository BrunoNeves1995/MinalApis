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

