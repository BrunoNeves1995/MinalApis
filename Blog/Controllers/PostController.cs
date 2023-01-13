using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.ViewModels.Pots;
using Blog.ViewModels.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using introducao.Model;

namespace Blog.Controllers
{   
    [ApiController]
    public class PostController : ControllerBase
    {
        
        [HttpGet("v1/posts")]
        public async Task<IActionResult> GetAsync(
            [FromServices] DataContext context,
            [FromQuery] int page = 0,
            [FromQuery] int pageZize = 25
        )
        {
            try
            {   
                var count = await context.Posts!.AsNoTracking().CountAsync();

                var posts = await context
                    .Posts!
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Include(x => x.User)
                    // .Select(x => 
                    // new ListPotsViewModel
                    // {
                    //     Id = x.Id,
                    //     Title = x.Title,
                    //     Slug = x.Slug,
                    //     LastUpdateDate = x.LastUpdateDate,
                    //     Category = x.Category!.Name,
                    //     User = $"{x.User!.Name} ({x.User.Email})"
                    // }
                    // )
                    .Skip(page * pageZize)
                    .Take(pageZize)
                    .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(data:
                    new 
                    {
                        totalCount = count,
                        page,
                        pageZize,
                        posts
                    }));
            }
            catch(SqlException ex) 
            {
                return  StatusCode(500, new ResultViewModel<string>(erro: $"01XE14 -{ex.Message} porque não existe uma coluna com esse nome na tabela post"));

            }
            catch (System.Exception)
            {
                
                 return StatusCode(500, new ResultViewModel<string>(erro: "01XE14 - Falha interna no servidor"));

            }
        }

        [HttpGet("v1/posts/{id:int}")]
        public async Task<IActionResult> DetailsAsync(
            [FromServices] DataContext context,
            [FromRoute] int id
            
        )
        {
            try
            {   
                var count = await context.Posts!.AsNoTracking().CountAsync();

                var post = await context
                    .Posts!
                    .AsNoTracking()
                    .Include(x => x.User)
                        .ThenInclude(x => x!.Roles) // sub documentos -> gera sub select, devemos evitar
                    .Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == id);
                if(post is null)
                    return NotFound(new ResultViewModel<string>(erro: "Pots não encontrados"));

                return Ok(new ResultViewModel<Post>(data: post));

                
            }
            catch (System.Exception)
            {
                
                return StatusCode(500, new ResultViewModel<string>(erro: "01XE14 - Falha interna no servidor"));
            }
        }

        [HttpGet("v1/posts/category/{category}")]
        public async Task<IActionResult> GetByCateg6ryAsync(
            [FromServices] DataContext context,
            [FromRoute] string category,
            [FromQuery] int page = 0,
            [FromQuery] int pageZize = 25
        )
        {
            try
            {   
                var count = await context.Posts!.AsNoTracking().CountAsync();

                var posts = await context
                    .Posts!
                    .AsNoTracking()
                    .Include(x => x.User)
                    .Include(x => x.Category)
                    .Where(x => x.Category!.Slug == category)
                    .Select(x => 
                    new ListPotsViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Slug = x.Slug,
                        LastUpdateDate = x.LastUpdateDate,
                        Category = x.Category!.Name,
                        User = $"{x.User!.Name} ({x.User.Email})"
                    }
                    )
                    .Skip(page * pageZize)
                    .Take(pageZize)
                    .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(data:
                    new 
                    {
                        totalCount = count,
                        page,
                        pageZize,
                        posts
                    }));
            }
            catch(SqlException) 
            {
                return  StatusCode(500, new ResultViewModel<string>(erro: $"01XE14 Ocorreu um erro interno no banco de dados"));

            }
            catch (System.Exception)
            {
                
                 return StatusCode(500, new ResultViewModel<string>(erro: "01XE14 - Falha interna no servidor"));

            }
        }
    }
}