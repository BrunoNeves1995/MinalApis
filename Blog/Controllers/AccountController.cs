using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Blog.Services;
using Blog.ViewModels.Result;
using introducao.Model;
using Blog.Data;
using Blog.Externsions;
using SecureIdentity.Password;
using Microsoft.EntityFrameworkCore;
using Blog.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Controllers

{
    [ApiController]
    public class AccountController : ControllerBase
    {
        // private readonly TokenService _tokenService;
        // public AccountController(TokenService tokenService)
        // {
        //     _tokenService = tokenService;
        // }

         [HttpPost("v1/account")]
         public async Task<IActionResult> Post(
            [FromBody] RegisterViewModel model,
            [FromServices] DataContext context,
             [FromServices] EmailService emailService
         )
         {
            if(!ModelState.IsValid)
               return BadRequest(new ResultViewModel<string>(errors: ModelState.GetErrors()));
            
            try
            {
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Slug = model.Slug
                };

                // gerando uma senha forte
                var senha = PasswordGenerator.Generate(32);
                user.PasswordHash = PasswordHasher.Hash(password: senha);

                await context.AddAsync(user);
                await context.SaveChangesAsync();

                emailService.Send
                (
                    user.Name,
                    user.Email,
                    subject: "Bem vindo ao blog",
                    body: $"\nSua senha é <Strong>{senha}</Strong>"                
                    );

                return Ok(new ResultViewModel<dynamic>(data: new 
                {
                    user  =  user.Email, 
                    senha
                }));
            } 
            catch(DbUpdateException)
            {
               return StatusCode(400, new ResultViewModel<string>(erro: "01XE9 - Esse e-mail ja esta cadastrado"));
            }
            catch (System.Exception)
            {
                
                return StatusCode(500, new ResultViewModel<string>(erro: "01XE10 - Falha interna no servidor"));
            }
                
         }


        [HttpPost("v1/account/login")]
        public async Task<IActionResult> Login(
            [FromServices] DataContext context,
            [FromServices] TokenService tokenService,
            [FromBody] LoginViewModel model
            )
        {   
            if(!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(errors: ModelState.GetErrors()));

            try
            {   
                var user = await context
                    .Users
                    !.AsNoTracking()
                    .Include(x => x.Roles)
                    .FirstOrDefaultAsync(x => x.Email == model.Email);
                // aqui o usuario não existe
                if(user is null)
                    return StatusCode(401, new ResultViewModel<string>(erro: "Usuario ou senha inválidos"));

                // verificando a senha e comparando o rash das senhas
               if(!PasswordHasher.Verify(user.PasswordHash!, model.Password))
                    return StatusCode(401, new ResultViewModel<string>(erro: "Usuarios ou senha inválidos"));            


                var token = tokenService.GeradorDeToken(user: user);
                return Ok(new ResultViewModel<String>(data: token));
            }
            catch (Exception)
            {

               return StatusCode(500, new ResultViewModel<string>(erro: "01XE11 - Falha interna no servidor"));
            }
        }

        // Testando os Roles //

        // [Authorize(Roles = "user")]
        // [HttpGet("v1/user")]
        // public IActionResult GetUser() => Ok(new ResultViewModel<string>(data: User.Identity?.Name!));


        // [Authorize(Roles = "author")]
        // [HttpGet("v1/author")]
        // public IActionResult GetAuthor() => Ok(new ResultViewModel<string>(data: User.Identity?.Name!));


        
        // [Authorize(Roles = "admin")]
        // [HttpGet("v1/admin")]
        // public IActionResult GetAdmin() => Ok(new ResultViewModel<string>(data: User.Identity?.Name!));


        [Authorize]
        [HttpPost("v1/account/upload-image")]
        public async Task<IActionResult> UploadImage(
            [FromServices] DataContext context,
            [FromBody] UploadImageViewModel model
        )
        {
           
            var fileName = $"{Guid.NewGuid().ToString().Substring(0, 10)}.jpg";
            
            try
            {
                var data = new Regex(pattern: @"^data:image\/[a-z]+;base64,").Replace(model.Base64Image, "");

                // convertendo a imagem para bytes
                var bytes = Convert.FromBase64String(data);

                // salvando image no disco
                await System.IO.File.WriteAllBytesAsync(path: $"wwwroot/images/{fileName}", bytes);

            }
            catch (System.Exception)
            {
                return StatusCode(500, new ResultViewModel<string>(erro: "01XE12 - Falha interna no servidor"));
            }

            // atualizando o usuario
            var user = await context
                .Users!
                .FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);
            
            if(user == null)
            {
                return NotFound(new ResultViewModel<User>(erro: "Usuario não encontrado"));
            }
            

            user.Image = $"https://localhost:3000/images/{fileName}";

            try
            {
                context.Users!.Update(user);
                await context.SaveChangesAsync();
                return Ok(new ResultViewModel<string>(data: "Imagem alterada com sucesso!"));
            }
            catch (System.Exception)
            {
                
                return StatusCode(500, new ResultViewModel<string>(erro: "01XE13 - Falha interna no servidor"));
            }
        }
    }
}